using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WindowCommandAttribute : Attribute
    {
        readonly string displayNameTranslationKey;

        public WindowCommandAttribute(string displayNameTranslationKey)
        {
            this.displayNameTranslationKey = displayNameTranslationKey;
        }

        public string DisplayNameTranslationKey
        {
            get { return displayNameTranslationKey; }
        }

        public string? MenuLocation { get; set; }
    }
}
