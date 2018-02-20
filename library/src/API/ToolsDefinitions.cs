using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.API
{
    internal static class ToolsDefinitions
    {
#if __iOS
        private const string DllName = "__Internal";
#else
        private const string DllName = "world";
#endif
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void wavwrite([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, int nbit, [MarshalAs(UnmanagedType.LPStr)] string filename);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudioLength([MarshalAs(UnmanagedType.LPStr)] string filename);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void wavread([MarshalAs(UnmanagedType.LPStr)] string filename,
            out int fs, out int nbit, [In][Out] IntPtr x);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteF0([MarshalAs(UnmanagedType.LPStr)] string filename, int f0_length, double frame_period,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] temporal_positions,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] f0, int text_flag);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadF0([MarshalAs(UnmanagedType.LPStr)] string filename,
            [In][Out] IntPtr temporal_positions, [In][Out] IntPtr f0);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetHeaderInformation([MarshalAs(UnmanagedType.LPStr)] string filename,
            [MarshalAs(UnmanagedType.LPStr)] string parameter);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteSpectralEnvelope([MarshalAs(UnmanagedType.LPStr)] string filename, int fs, int f0_length,
            double frame_period, int fft_size, int number_of_dimensions, [In] IntPtr[] spectrogram);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadSpectralEnvelope([MarshalAs(UnmanagedType.LPStr)] string filename, [In][Out] IntPtr[] spectrogram);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteAperiodicity([MarshalAs(UnmanagedType.LPStr)] string filename, int fs, int f0_length,
            double frame_period, int fft_size, int number_of_dimensions, [In] IntPtr[] aperiodicity);
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadAperiodicity([MarshalAs(UnmanagedType.LPStr)] string filename, [In][Out] IntPtr[] aperiodicity);
    }
}