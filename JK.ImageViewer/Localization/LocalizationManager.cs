using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer.Localization
{
    public static class LocalizationManager
    {
        public class LanguageItem(string id, string displayName)
        {
            public string Id { get; init; } = id;
            public string DisplayName { get; init; } = displayName;

            public override string ToString()
            {
                return $"{nameof(LanguageItem)} {{ {nameof(Id)} = {Id}, {nameof(DisplayName)} = {DisplayName} }}";
            }
        }

        const string FALLBACK_LANGUAGE = "en-US";

        public static event EventHandler? LanguageChanged;

        private static Dictionary<string, string>? currentLocaleStrings;
        private static Dictionary<string, string>? fallbackStrings;

        public static string LanguageDirectory => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Localization",
            "Languages"
        );

        private static void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }

        private static string GetLangFilePath(string lang)
        {
            return Path.Combine(
                LanguageDirectory,
                lang + ".xml"
            );
        }

        public static LanguageItem[] GetAvailableLanguages()
        {
            return Directory.GetFiles(LanguageDirectory)
                .Where(path => Path.GetExtension(path)?.Equals(".xml", StringComparison.InvariantCultureIgnoreCase) ?? false)
                .Select(path => new LanguageItem(
                    Path.GetFileNameWithoutExtension(path) ?? "",
                    XDocument.Load(path).Root?.Attribute("Name")?.Value ?? Path.GetFileNameWithoutExtension(path) ?? ""
                ))
                .ToArray();
        }

        public static string GetString(string key)
        {
            if (currentLocaleStrings?.ContainsKey(key) ?? false)
                return currentLocaleStrings[key];
            if (fallbackStrings?.ContainsKey(key) ?? false)
                return fallbackStrings[key];
            return key;
        }

        public static string _(this object any, string key)
        {
            return GetString(key);
        }

        private static Dictionary<string, string>? LoadLocaleInternal(string path)
        {
            if (!File.Exists(path))
                return null;

            var doc = XDocument.Load(path);

            // TODO

            return doc.Root!
                .Elements("String")
                .ToDictionary(x => x.Attribute("Key")!.Value, x => x.Value);
        }

        public static void LoadLocale(string name)
        {
            if (fallbackStrings is null)
                fallbackStrings = LoadLocaleInternal(GetLangFilePath(FALLBACK_LANGUAGE));

            currentLocaleStrings = LoadLocaleInternal(GetLangFilePath(name));

            OnLanguageChanged();
        }
    }
}
