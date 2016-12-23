using System;
using System.Runtime.InteropServices;
using DotnetWorld.API.Common.Struct;

namespace DotnetWorld.API.Linux
{
    internal static class CoreDefinitions
    {
        private const string DllName = "libworld.so";
        #region CheapTrick
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void CheapTrick([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] time_axis,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] f0, int f0_length, [In] CheapTrickOption option,
            [In][Out] IntPtr[] spectrogram);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeCheapTrickOption([Out] CheapTrickOption option);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetFFTSizeForCheapTrick(int fs, [Out] CheapTrickOption option);
        #endregion

        #region D4C
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void D4C([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] time_axis,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] f0, int f0_length, int fft_size, [In] D4COption option,
            [In][Out] IntPtr[] aperiodicity);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeD4COption([Out] D4COption option);
        #endregion

        #region Dio
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void Dio([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, [In] DioOption option,
            [In][Out] IntPtr time_axis, [In][Out] IntPtr f0);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeDioOption([Out] DioOption option);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSamplesForDIO(int fs, int x_length, double frame_period);
        #endregion

        #region Harvest
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void Harvest([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, [In] HarvestOption option,
            [In][Out] IntPtr time_axis, [In][Out] IntPtr f0);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeHarvestOption([Out] HarvestOption option);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSamplesForHarvest(int fs, int x_length, double frame_period);
        #endregion

        #region StoneMask
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void StoneMask([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] x,
            int x_length, int fs, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] time_axis,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] f0, int f0_length,
            [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] double[] refined_f0);
        #endregion

        #region Synthesis
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void Synthesis([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] double[] f0,
            int f0_length, [In] IntPtr[] spectrogram, [In] IntPtr[] aperiodicity, int fft_size, double frame_period, int fs,
            int y_length,
            [In][Out] IntPtr y);
        #endregion

        #region SynthesisRealtime - Not Supported
        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeSynthesizer(int fs, double frame_period, int fft_size,
            int buffer_size, int number_of_pointers, [Out] WorldSynthesizer synth);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int AddParameters([In] double[] f0, int f0_length, ref IntPtr spectrogram,
            ref IntPtr aperiodicity, [In][Out] WorldSynthesizer synth);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void RefreshSynthesizer([In][Out] WorldSynthesizer synth);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroySynthesizer([In][Out] WorldSynthesizer synth);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int IsLocked([In] WorldSynthesizer synth);

        [DllImport(DllName,CallingConvention = CallingConvention.Cdecl)]
        public static extern int Synthesis2([In][Out] WorldSynthesizer synth);
        #endregion
    }
}