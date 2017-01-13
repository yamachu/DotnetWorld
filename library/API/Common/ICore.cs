using System;
using DotnetWorld.API.Common.Struct;

namespace DotnetWorld.API.Common
{
    public interface ICore
    {
        void CheapTrick(double[] x, int x_length, int fs, double[] temporal_positions,
            double[] f0, int f0_length, CheapTrickOption option,
            double[,] spectrogram);

        void InitializeCheapTrickOption(int fs, CheapTrickOption option);

        int GetFFTSizeForCheapTrick(int fs, CheapTrickOption option);

        void D4C(double[] x, int x_length, int fs, double[] time_axis,
            double[] f0, int f0_length, int fft_size, D4COption option,
            double[,] aperiodicity);

        void InitializeD4COption(D4COption option);

        void Dio(double[] x, int x_length, int fs, DioOption option,
            double[] time_axis, double[] f0);

        void InitializeDioOption(DioOption option);

        int GetSamplesForDIO(int fs, int x_length, double frame_period);

        void Harvest(double[] x, int x_length, int fs,
            HarvestOption option, double[] time_axis, double[] f0);

        void InitializeHarvestOption(HarvestOption option);

        int GetSamplesForHarvest(int fs, int x_length, double frame_period);

        void StoneMask(double[] x, int x_length, int fs, double[] time_axis,
            double[] f0, int f0_length, double[] refined_f0);

        void Synthesis(double[] f0, int f0_length, double[,] spectrogram,
            double[,] aperiodicity, int fft_size, double frame_period, int fs,
            int y_length, double[] y);

        void InitializeSynthesizer(int fs, double frame_period, int fft_size,
            int buffer_size, int number_of_pointers, WorldSynthesizer synth);

        bool AddParameters(double[] f0, int f0_length, double[,] spectrogram,
            double[,] aperiodicity, WorldSynthesizer synth);

        void RefreshSynthesizer(WorldSynthesizer synth);

        void DestroySynthesizer(WorldSynthesizer synth);

        bool IsLocked(WorldSynthesizer synth);

        bool Synthesis2(WorldSynthesizer synth);
    }
}