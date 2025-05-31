using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer.Theming
{
    public class Theme
    {
        public required IconMapping IconMapping;
        public required SystemColorMode ThemeColorMode;

        protected string? ThemeDirPath;

        public static Theme LoadFromFile(string themeXmlPath)
        {
            var themeDirPath = Path.GetDirectoryName(themeXmlPath);

            var doc = XDocument.Load(themeXmlPath);

            // TODO Assert root tag name and format version

            IconMapping iconMapping = new();
            var themeColorMode = SystemColorMode.System;

            var rootColorSchemeAttribute = doc.Root?.Attribute("ColorMode")?.Value;
            if (rootColorSchemeAttribute is not null)
                themeColorMode = Enum.Parse<SystemColorMode>(rootColorSchemeAttribute);

            var xIconMapping = doc.Root?.Element("Icons");
            if (xIconMapping is not null)
            {
                foreach (var xMappingEntry in xIconMapping.Elements("Icon"))
                {
                    var keyAttrib = xMappingEntry.Attribute("Key")?.Value;
                    if (keyAttrib is null)
                        throw new Exception("Key attribute missing");

                    var directImageChild = xMappingEntry.Element("Image");
                    var conditionalChildren = xMappingEntry.Elements().Where(x =>
                        x.Name == "IfColorScheme"
                    ).ToArray();

                    List<IconMappingImage> images = new();
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

                    iconMapping.Add(keyAttrib, new(
                        keyAttrib,
                        images.ToArray()
                    ));
                }
            }
            else Debug.WriteLine("No command icon mappings defined in theme");

            return new Theme()
            {
                IconMapping = iconMapping,
                ThemeColorMode = themeColorMode,

                ThemeDirPath = themeDirPath,
            };
        }

        public Image? GetImage(string key)
        {
            if (!IconMapping.ContainsKey(key))
            {
                Debug.WriteLine($"GetImageForCommand(\"{key}\") -> No mapping for key");
                return null;
            }
            if (ThemeDirPath is null)
            {
                Debug.WriteLine($"GetImageForCommand(\"{key}\") -> No reference path");
                return null;
            }

            var mappingEntry = IconMapping[key];
            var source = mappingEntry.Images.Where(i => i.Condition.Evaluate()).FirstOrDefault()?.Source;
            if (source is null)
            {
                Debug.WriteLine($"GetImageForCommand(\"{key}\") -> No matching image");
                return null;
            }

            var fullSource = Path.Combine(ThemeDirPath, source);

            if (!File.Exists(fullSource))
            {
                Debug.WriteLine($"GetImageForCommand(\"{key}\") -> Path {fullSource} does not exist");
                return null;
            }

            try
            {
                using var magickImage = new MagickImage(fullSource);
                return magickImage.ToBitmap();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetImageForCommand(\"{key}\") -> Exception:");
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
