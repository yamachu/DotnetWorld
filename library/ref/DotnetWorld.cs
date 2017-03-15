using System;
using DotnetWorld.API.Structs;

namespace DotnetWorld.API
{
    #region World original APIs
    public class Core
    {
        public static void CheapTrick(double[] x, int x_length, int fs, double[] temporal_positions,
            double[] f0, int f0_length, CheapTrickOption option,
            double[,] spectrogram) {}

        public static void InitializeCheapTrickOption(int fs, CheapTrickOption option) {}

        public static int GetFFTSizeForCheapTrick(int fs, CheapTrickOption option) { throw null; }

        public static void D4C(double[] x, int x_length, int fs, double[] time_axis,
            double[] f0, int f0_length, int fft_size, D4COption option,
            double[,] aperiodicity) {}
        public static void InitializeD4COption(D4COption option) {}

        public static void Dio(double[] x, int x_length, int fs, DioOption option,
            double[] time_axis, double[] f0) {}

        public static void InitializeDioOption(DioOption option) {}

        public static int GetSamplesForDIO(int fs, int x_length, double frame_period) { throw null; }

        public static void Harvest(double[] x, int x_length, int fs, HarvestOption option, double[] time_axis, double[] f0) {}
        
        public static void InitializeHarvestOption(HarvestOption option) {}

        public static int GetSamplesForHarvest(int fs, int x_length, double frame_period) { throw null; }

        public static void StoneMask(double[] x, int x_length, int fs, double[] time_axis, double[] f0, int f0_length, double[] refined_f0) {}

        public static void Synthesis(double[] f0, int f0_length, double[,] spectrogram, double[,] aperiodicity, int fft_size, double frame_period, int fs, int y_length, double[] y) {}
        
        public static void InitializeSynthesizer(int fs, double frame_period, int fft_size, int buffer_size, int number_of_pointers, WorldSynthesizer synth) {}
        
        public static bool AddParameters(double[] f0, int f0_length, double[,] spectrogram, double[,] aperiodicity, WorldSynthesizer synth) { throw null; }
        
        public static void RefreshSynthesizer(WorldSynthesizer synth) {}

        public static void DestroySynthesizer(WorldSynthesizer synth) {}

        public static bool IsLocked(WorldSynthesizer synth) { throw null; }

        public static bool Synthesis2(WorldSynthesizer synth) { throw null; }
    }

    public partial class Tools
    {
        public static int GetAudioLength(string filename) { throw null; }
            
        public static void WavRead(string filename, out int fs, out int nbit, double[] x) { throw null; }
        
        public static void WavWrite(double[] x, int x_length, int fs, int nbit, string filename) {}
    }
    #endregion

    #region User added APIs
    public partial class Tools
    {
        public static int GetRawAudioLength(string filename, int nbit) { throw null; }
        public static void RawRead(string filename, int nbit, double[] x) {}
    }
    #endregion
}

namespace DotnetWorld.API.Structs
{
    public class CheapTrickOption
    {
        public double q1;
        public double f0_floor;
        public int fft_size;
    }

    public class D4COption
    {
        public double threshold;
    }

    public class DioOption
    {
        public double f0_floor;
        public double f0_ceil;
        public double channels_in_octave;
        public double frame_period;
        public int speed;
        public double allowed_range;
    }

    public class HarvestOption
    {
        public double f0_floor;
        public double f0_ceil;
        public double frame_period;
    }

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
        public static void CopyFromBufferToArray(this WorldSynthesizer synthesiser, double[] arr) { throw null; }
    }
}
