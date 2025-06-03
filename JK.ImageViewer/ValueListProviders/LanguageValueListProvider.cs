using JK.ImageViewer.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.ValueListProviders
{
    public class LanguageValueListProvider : IValueListProvider
    {
        public string DisplayMember => nameof(LocalizationManager.LanguageItem.DisplayName);
        public string ValueMember => nameof(LocalizationManager.LanguageItem.Id);

        public object?[] GetValues()
        {
            return LocalizationManager
                .GetAvailableLanguages()
                //.Select(l => (object?)new KeyValuePair<string, string>(l.Id, l.DisplayName))
                .Cast<object?>()
                .ToArray();
        }
    }
}
