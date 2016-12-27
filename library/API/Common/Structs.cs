using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.API.Common.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public class CheapTrickOption
    {
        public double q1;
        public double f0_floor;
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
        int fs;
        double frame_period;
        int buffer_size;
        int number_of_pointers;
        int fft_size;

        IntPtr buffer; // double*
        int current_pointer;
        int i;

        IntPtr f0_length; // int*
        IntPtr f0_origin; // int*
        IntPtr spectrogram; // double***
        IntPtr aperiodicity; // double***

        int cumulative_frame;
        int current_frame;

        IntPtr interpolated_vuv; // double**
        IntPtr pulse_locations; // double**
        IntPtr pulse_locations_index; // int**
        IntPtr number_of_pulses; // int*

        IntPtr impulse_response; // double*

        IntPtr minimum_phase; // MinimumPhaseAnalysis - not supported?
        IntPtr inverse_real_fft; // InverseRealFFT
        IntPtr forward_real_fft; // ForwardRealFFT
    }
}