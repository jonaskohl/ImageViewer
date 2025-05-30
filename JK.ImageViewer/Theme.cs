using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public class Theme
    {
        public required CommandIconMapping CommandIconMapping;

        protected string? ThemeDirPath;

        public static Theme LoadFromFile(string themeXmlPath)
        {
            var themeDirPath = Path.GetDirectoryName(themeXmlPath);

            var doc = XDocument.Load(themeXmlPath);

            // TODO Assert root tag name and format version

            CommandIconMapping commandIconMapping = new();

            var xIconMapping = doc.Root?.Element("CommandIconMapping");
            if (xIconMapping is not null)
            {
                foreach (var xMappingEntry in xIconMapping.Elements("Icon"))
                {
                    var commandAttrib = xMappingEntry.Attribute("Command")?.Value;
                    if (commandAttrib is null)
                        throw new Exception("Command attribute missing");

                    var directImageChild = xMappingEntry.Element("Image");
                    var conditionalChildren = xMappingEntry.Elements().Where(x =>
                        x.Name == "IfColorScheme"
                    ).ToArray();

                    List<CommandIconMappingImage> images = new();
                    if (conditionalChildren.Length > 0 && directImageChild is not null)
                        throw new Exception("Invalid combination of child elements");
                    else if (directImageChild is not null)
                    {
                        images.Add(new(
                            directImageChild.Attribute("Source")!.Value,
                            AlwaysTrueCondition.Instance
                        ));
                    }
                    else
                    {
                        foreach (var cond in conditionalChildren)
                        {
                            var name = cond.Name.ToString();
                            switch (name)
                            {
                                case "IfColorScheme":
                                    images.Add(new(
                                        cond.Element("Image")!.Attribute("Source")!.Value,
                                        new ColorSchemeCondition(cond.Attribute("Which")!.Value)
                                    ));
                                    break;
                                default:
                                    throw new Exception("Invalid child");
                            }
                        }
                    }

                    commandIconMapping.Add(commandAttrib, new(
                        commandAttrib,
                        images.ToArray()
                    ));
                }
            }
            else Debug.WriteLine("No command icon mappings defined in theme");

            return new Theme()
            {
                CommandIconMapping = commandIconMapping,

                ThemeDirPath = themeDirPath,
            };
        }

        public Image? GetImageForCommand(string commandName)
        {
            if (!CommandIconMapping.ContainsKey(commandName))
            {
                Debug.WriteLine($"GetImageForCommand(\"{commandName}\") -> No mapping for command");
                return null;
            }
            if (ThemeDirPath is null)
            {
                Debug.WriteLine($"GetImageForCommand(\"{commandName}\") -> No reference path");
                return null;
            }

            var mappingEntry = CommandIconMapping[commandName];
            var source = mappingEntry.Images.Where(i => i.Condition.Evaluate()).FirstOrDefault()?.Source;
            if (source is null)
            {
                Debug.WriteLine($"GetImageForCommand(\"{commandName}\") -> No matching image");
                return null;
            }

            var fullSource = Path.Combine(ThemeDirPath, source);

            if (!File.Exists(fullSource))
            {
                Debug.WriteLine($"GetImageForCommand(\"{commandName}\") -> Path {fullSource} does not exist");
                return null;
            }

            try
            {
                using var magickImage = new MagickImage(fullSource);
                return magickImage.ToBitmap();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetImageForCommand(\"{commandName}\") -> Exception:");
                Debug.WriteLine(ex);
                return null;
            }
        }
    }

    public class CommandIconMapping : Dictionary<string, CommandIconMappingEntry> { }

    public record CommandIconMappingEntry(
        string Command,
        CommandIconMappingImage[] Images
    );

    public record CommandIconMappingImage(
        string Source,
        ThemePropertyCondition Condition
    );

    public abstract class ThemePropertyCondition
    {
        public abstract bool Evaluate();
    }

    public enum ColorSchemeKind
    {
        Light,
        Dark,
        Any
    }

    public class ColorScheme(string scheme)
    {
        public ColorSchemeKind Kind { get; protected set; } = scheme switch
        {
            var s when s.ToLowerInvariant() == "light" => ColorSchemeKind.Light,
            var s when s.ToLowerInvariant() == "dark" => ColorSchemeKind.Dark,
            var s when s.ToLowerInvariant() == "any" || s == "*" => ColorSchemeKind.Any,
            _ => throw new Exception($"Invalid color scheme {scheme}")
        };

        public bool IsActive()
        {
            switch (Kind)
            {
                case ColorSchemeKind.Light:
                    return !Application.IsDarkModeEnabled;
                case ColorSchemeKind.Dark:
                    return Application.IsDarkModeEnabled;
                case ColorSchemeKind.Any:
                    return true;
                default:
                    return false;
            }
        }
    }

    public class AlwaysTrueCondition : ThemePropertyCondition
    {
        public override bool Evaluate()
        {
            return true;
        }

        private AlwaysTrueCondition() { }

        private static AlwaysTrueCondition? _instance;
        public static AlwaysTrueCondition Instance => _instance ?? new();
    }

    public class ColorSchemeCondition(string colorSchemeProperty) : ThemePropertyCondition
    {
        public ColorScheme[] ColorSchemes { get; private set; } = colorSchemeProperty
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => new ColorScheme(s))
            .ToArray();

        public override bool Evaluate()
        {
            return ColorSchemes.Any(s => s.IsActive());
        }
    }
}
