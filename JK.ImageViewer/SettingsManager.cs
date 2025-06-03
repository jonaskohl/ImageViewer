using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            Environment.ExpandEnvironmentVariables(@"%appdata%\Jonas Kohl\ImageViewer"),
            "Preferences.xml"
        );

        private SettingsManager()
        {
            var doc = File.Exists(PreferencesPath)
                ? XDocument.Load(PreferencesPath)
                : null;

            // TODO

            settings = doc?.Root?
                .Elements("Setting")
                .ToDictionary(
                    el => el.Attribute("Key")!.Value,
                    el => el.Attribute("Value")!.Value
                ) ?? new();
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

        public object GetPreference(string key, Type targetType, object defaultValue)
        {
            if (!settings.ContainsKey(key))
                return defaultValue;
            try
            {
                return TypeDescriptor.GetConverter(targetType).ConvertFrom(settings[key]) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public string SerializeValue<T>(T value)
        {
            return SerializeValue(typeof(T), value);
        }

        public string SerializeValue(object? value)
        {
            return SerializeValue(value?.GetType() ?? typeof(object), value);
        }

        public string SerializeValue(Type valueType, object? value)
        {
            return TypeDescriptor.GetConverter(valueType).ConvertToString(value) ?? "";
        }

        public void SetPreference<T>(string key, T value) where T : IConvertible
        {
            SetSerializedPreference(key, SerializeValue(value));
        }

        public void SetSerializedPreference(string key, string value)
        {
            var oldValue = settings.ContainsKey(key) ? settings[key] : null;
            settings[key] = value;

            Debug.WriteLine($"SettingChanged:> (Key={key}) (OldValue={oldValue}) (NewValue={value})");

            SettingChanged?.Invoke(null, new SettingChangedEventArgs()
            {
                Key = key,
                OldValue = oldValue,
                NewValue = value,
            });
        }

        public void Save()
        {
            var dir = Path.GetDirectoryName(PreferencesPath);
            if (dir is not null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

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
