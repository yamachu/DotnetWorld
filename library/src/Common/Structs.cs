using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.API.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public class CheapTrickOption
    {
        public double q1;
        public double f0_floor;
        public int fft_size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class D4COption
    {
        public double threshold;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DioOption
    {
        public double f0_floor;
        public double f0_ceil;
        public double channels_in_octave;
        public double frame_period;
        public int speed;
        public double allowed_range;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class HarvestOption
    {
        public double f0_floor;
        public double f0_ceil;
        public double frame_period;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class WorldSynthesizer
    {
        public int fs;
        public double frame_period;
        public int buffer_size;
        public int number_of_pointers;
        public int fft_size;

        public IntPtr _buffer; // double*
        public int current_pointer;
        public int i;

        internal IntPtr dc_remover; // double*

        internal IntPtr f0_length; // int*
        internal IntPtr f0_origin; // int*
        internal IntPtr spectrogram; // double***
        internal IntPtr aperiodicity; // double***

        internal int handoff;
        internal double handoff_phase;
        internal double handoff_f0;
        internal int last_location;

        public int cumulative_frame;
        public int current_frame;

        internal IntPtr interpolated_vuv; // double**
        internal IntPtr pulse_locations; // double**
        internal IntPtr pulse_locations_index; // int**
        internal IntPtr number_of_pulses; // int*

        internal IntPtr impulse_response; // double*

        internal IntPtr minimum_phase; // MinimumPhaseAnalysis
        internal IntPtr inverse_real_fft; // InverseRealFFT
        internal IntPtr forward_real_fft; // ForwardRealFFT
    }

    public static class WorldSynthesizerExtension
    {
        public static void CopyFromBufferToArray(this WorldSynthesizer synthesiser, double[] arr)
        {
            if (arr.Length < synthesiser.buffer_size)
            {
                throw new Exception();
            }
            Marshal.Copy(synthesiser._buffer, arr, 0, synthesiser.buffer_size);
        }
    }

    #region FFT
    [StructLayout(LayoutKind.Sequential)]
    internal struct FFT_PLAN
    {
        public int n;
        public int sign;
        public uint flags;
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] c_in;
        public IntPtr _in; // double* : fft_complex
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] c_out;
        public IntPtr _out; // double* : fft_complex
        public IntPtr input; // double*
        public IntPtr ip; // int*
        public IntPtr w; // double*
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ForwardRealFFT
    {
        public int fft_size;
        public IntPtr waveform; // double*
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] spectrum;
        public FFT_PLAN forward_fft;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct InverseRealFFT
    {
        public int fft_size;
        public IntPtr waveform; // double*
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] spectrum;
        public FFT_PLAN inverse_fft;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct InverseComplexFFT
    {
        public int fft_size;
        public IntPtr input; // double[fft_size]
        public IntPtr output; // double[fft_size]
        public FFT_PLAN inverse_fft;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MinimumPhaseAnalysis
    {
        public int fft_size;
        public IntPtr log_spectrum; // double*
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] minimum_phase_spectrum;
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
        public double[] cepstrum;
        public FFT_PLAN inverse_fft;
        public FFT_PLAN forward_fft;
    }
    #endregion
}