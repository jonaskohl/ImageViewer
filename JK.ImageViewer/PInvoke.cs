using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public static partial class PInvoke
    {
        [LibraryImport("uxtheme.dll", StringMarshalling = StringMarshalling.Utf16)]
        public static partial uint SetWindowTheme(nint hwnd, string? pszSubAppName, string? pszSubIdList);
    }
}
