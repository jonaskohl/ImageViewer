using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public class SettingsManager
    {
        private static SettingsManager? _instance;
        public static SettingsManager Instance => _instance ??= new();

        private Dictionary<string, string> settings;

        public class SettingChangedEventArgs : EventArgs
        {
            public required string Key { get; init; }
            public required string? OldValue { get; init; }
            public required string NewValue { get; init; }
        }

        public event EventHandler<SettingChangedEventArgs>? SettingChanged;

        public static void EnsureInstance()
        {
            _instance ??= new();
        }

        public string PreferencesPath => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Settings",
            "Preferences.xml"
        );

        private SettingsManager()
        {
            var doc = XDocument.Load(PreferencesPath);

            // TODO

            settings = doc.Root!
                .Elements("Setting")
                .ToDictionary(
                    el => el.Attribute("Key")!.Value,
                    el => el.Attribute("Value")!.Value
                );
        }

        public T GetPreference<T>(string key, T defaultValue) where T : IConvertible
        {
            if (!settings.ContainsKey(key))
                return defaultValue;
            try
            {
                return (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(settings[key]) ?? defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public void SetPreference<T>(string key, T value) where T : IConvertible
        {
            var oldValue = settings.ContainsKey(key) ? settings[key] : null;
            var newValue = TypeDescriptor.GetConverter(typeof(T)).ConvertToString(value)!;
            settings[key] = newValue;
            SettingChanged?.Invoke(null, new SettingChangedEventArgs()
            {
                Key = key,
                OldValue = oldValue,
                NewValue = newValue,
            });
        }

        public void Save()
        {
            new XDocument(
                new XElement("Preferences",
                    settings
                        .Select(setting =>
                            new XElement("Setting",
                                new XAttribute("Key", setting.Key),
                                new XAttribute("Value", setting.Value)
                            )
                        )
                )
            ).Save(PreferencesPath);
        }
    }
}
