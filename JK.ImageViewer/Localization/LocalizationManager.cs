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
        const string FALLBACK_LANGUAGE = "en-US";

        private static Dictionary<string, string>? currentLocaleStrings;
        private static Dictionary<string, string>? fallbackStrings;

        private static string GetLangFilePath(string lang)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Localization",
                "Languages",
                lang + ".xml"
            );
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
        }
    }
}
