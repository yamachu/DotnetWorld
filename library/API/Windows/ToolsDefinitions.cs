using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.API.Windows
{
    internal static class ToolsDefinitions
    {
        private const string DllName = "world.dll";

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void wavwrite([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, int nbit, [MarshalAs(UnmanagedType.LPStr)] string filename);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudioLength([MarshalAs(UnmanagedType.LPStr)] string filename);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void wavread([MarshalAs(UnmanagedType.LPStr)] string filename,
            out int fs, out int nbit, [In][Out] IntPtr x);
    }
}