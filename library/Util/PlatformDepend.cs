using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.Utils
{
    internal static class PlatformDepend
    {
        public enum EnumPlatform
        {
            LINUX,
            OSX,
            WINDOWS,
        }

        public static EnumPlatform Platform
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return EnumPlatform.OSX;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return EnumPlatform.LINUX;
                }
                else
                {
                    return EnumPlatform.WINDOWS;
                }
            }
        }
    }
}
