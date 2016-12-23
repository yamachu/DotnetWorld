using System;
using System.Runtime.InteropServices;
using DotnetWorld.API.Common;
using DotnetWorld.API.Common.Struct;

namespace DotnetWorld.API.Linux
{
    public class Core: ICore
    {
        public void CheapTrick(double[] x, int x_length, int fs, double[] time_axis,
            double[] f0, int f0_length, CheapTrickOption option,
            double[,] spectrogram)
        {
            int outer = spectrogram.GetLength(0);
            int inner = spectrogram.GetLength(1);

            IntPtr[] ptrs_sp = new IntPtr[outer];

            for (var i = 0; i < outer; i++)
            {
                ptrs_sp[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());
            }

            CoreDefinitions.CheapTrick(x, x_length, fs, time_axis, f0, f0_length, option, ptrs_sp);

            var tmp_arr = new double[inner];
            
            for (var i = 0; i < spectrogram.GetLength(0); i++) {
                Marshal.Copy(ptrs_sp[i], tmp_arr, 0, inner);
                Buffer.BlockCopy(tmp_arr, 0, spectrogram, i * inner * sizeof(double), inner * sizeof(double));
                Marshal.FreeHGlobal(ptrs_sp[i]);
            }
        }

        public void InitializeCheapTrickOption(CheapTrickOption option)
            => CoreDefinitions.InitializeCheapTrickOption(option);

        public int GetFFTSizeForCheapTrick(int fs, CheapTrickOption option)
            => CoreDefinitions.GetFFTSizeForCheapTrick(fs, option);

        public void D4C(double[] x, int x_length, int fs, double[] time_axis, double[] f0, int f0_length, int fft_size, D4COption option, double[,] aperiodicity)
        {
            int outer = aperiodicity.GetLength(0);
            int inner = aperiodicity.GetLength(1);

            IntPtr[] ptrs_ap = new IntPtr[outer];

            for (var i = 0; i < outer; i++)
            {
                ptrs_ap[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());
            }

            CoreDefinitions.D4C(x, x_length, fs, time_axis, f0, f0_length, fft_size, option, ptrs_ap);

            var tmp_arr = new double[inner];
            
            for (var i = 0; i < aperiodicity.GetLength(0); i++) {
                Marshal.Copy(ptrs_ap[i], tmp_arr, 0, inner);
                Buffer.BlockCopy(tmp_arr, 0, aperiodicity, i * inner * sizeof(double), inner * sizeof(double));
                Marshal.FreeHGlobal(ptrs_ap[i]);
            }
        }

        public void InitializeD4COption(D4COption option)
            => CoreDefinitions.InitializeD4COption(option);

        public void Dio(double[] x, int x_length, int fs, DioOption option,
            double[] time_axis, double[] f0)
        {
            IntPtr ptr_time = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * time_axis.Length);
            IntPtr ptr_f0 = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * f0.Length);

            CoreDefinitions.Dio(x, x_length, fs, option, ptr_time, ptr_f0);

            Marshal.Copy(ptr_time, time_axis, 0, time_axis.Length);
            Marshal.Copy(ptr_f0, f0, 0, f0.Length);

            Marshal.FreeHGlobal(ptr_time);
            Marshal.FreeHGlobal(ptr_f0);
        }

        public void InitializeDioOption(DioOption option)
            => CoreDefinitions.InitializeDioOption(option);

        public int GetSamplesForDIO(int fs, int x_length, double frame_period)
            => CoreDefinitions.GetSamplesForDIO(fs, x_length, frame_period);

        public void Harvest(double[] x, int x_length, int fs, HarvestOption option, double[] time_axis, double[] f0)
        {
            IntPtr ptr_time = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * time_axis.Length);
            IntPtr ptr_f0 = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * f0.Length);
            
            CoreDefinitions.Harvest(x, x_length, fs, option, ptr_time, ptr_f0);

            Marshal.Copy(ptr_time, time_axis, 0, time_axis.Length);
            Marshal.Copy(ptr_f0, f0, 0, f0.Length);

            Marshal.FreeHGlobal(ptr_time);
            Marshal.FreeHGlobal(ptr_f0);
        }

        public void InitializeHarvestOption(HarvestOption option)
            => CoreDefinitions.InitializeHarvestOption(option);

        public int GetSamplesForHarvest(int fs, int x_length, double frame_period)
            => CoreDefinitions.GetSamplesForHarvest(fs, x_length, frame_period);

        public void StoneMask(double[] x, int x_length, int fs, double[] time_axis, double[] f0, int f0_length, double[] refined_f0)
            => CoreDefinitions.StoneMask(x, x_length, fs, time_axis, f0, f0_length, refined_f0);

        public void Synthesis(double[] f0, int f0_length, double[,] spectrogram, double[,] aperiodicity, int fft_size, double frame_period, int fs, int y_length, double[] y)
        {
            int outer = aperiodicity.GetLength(0);
            int inner = aperiodicity.GetLength(1);

            IntPtr[] ptrs_ap = new IntPtr[outer];
            IntPtr[] ptrs_sp = new IntPtr[outer];
            IntPtr ptr_y = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * y.Length);

            var tmp_arr = new double[inner];

            for (var i = 0; i < outer; i++)
            {
                ptrs_ap[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());
                ptrs_sp[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());

                Buffer.BlockCopy(spectrogram, i * inner * sizeof(double), tmp_arr, 0, inner * sizeof(double));
                Marshal.Copy(tmp_arr, 0, ptrs_sp[i], inner);
                Buffer.BlockCopy(aperiodicity, i * inner * sizeof(double), tmp_arr, 0, inner * sizeof(double));
                Marshal.Copy(tmp_arr, 0, ptrs_ap[i], inner);
            }

            CoreDefinitions.Synthesis(f0, f0_length, ptrs_sp, ptrs_ap, fft_size, frame_period, fs, y_length, ptr_y);

            for (var i = 0; i < aperiodicity.GetLength(0); i++) {
                Marshal.FreeHGlobal(ptrs_ap[i]);
                Marshal.FreeHGlobal(ptrs_sp[i]);
            }
            Marshal.Copy(ptr_y, y, 0, y.Length);

            Marshal.FreeHGlobal(ptr_y);
        }
    }
}