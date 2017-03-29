using System;
using System.Runtime.InteropServices;

namespace DotnetWorld.API
{
    public partial class Tools
    {
        public static int GetAudioLength(string filename)
            => ToolsDefinitions.GetAudioLength(filename);
            
        public static void WavRead(string filename, out int fs, out int nbit, double[] x)
        {
            IntPtr ptr_x = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * x.Length);
            ToolsDefinitions.wavread(filename, out fs, out nbit, ptr_x);
            Marshal.Copy(ptr_x, x, 0, x.Length);
            Marshal.FreeHGlobal(ptr_x);
        }

        public static void WavWrite(double[] x, int x_length, int fs, int nbit, string filename)
            => ToolsDefinitions.wavwrite(x, x_length, fs, nbit, filename);

        public static void WriteF0(string filename, int f0_length, double frame_period,
            double[] temporal_positions, double[] f0, int text_flag)
            => ToolsDefinitions.WriteF0(filename, f0_length, frame_period, temporal_positions, f0, text_flag);

        public static int ReadF0(string filename, double[] temporal_positions, double[] f0)
        {
            IntPtr ptr_temporal_positions = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * temporal_positions.Length);
            IntPtr ptr_f0 = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * f0.Length);
            var result = ToolsDefinitions.ReadF0(filename, ptr_temporal_positions, ptr_f0);
            Marshal.Copy(ptr_temporal_positions, temporal_positions, 0, temporal_positions.Length);
            Marshal.Copy(ptr_f0, f0, 0, f0.Length);
            Marshal.FreeHGlobal(ptr_temporal_positions);
            Marshal.FreeHGlobal(ptr_f0);

            return result;
        }

        public static double GetHeaderInformation(string filename, string parameter)
            => ToolsDefinitions.GetHeaderInformation(filename, parameter);

        public static void WriteSpectralEnvelope(string filename, int fs, int f0_length,
            double frame_period, int fft_size, int number_of_dimensions, double[,] spectrogram)
        {
            int outer = spectrogram.GetLength(0);
            int inner = spectrogram.GetLength(1);

            IntPtr[] ptr_sp = new IntPtr[outer];

            var tmp_arr = new double[inner];

            for (var i = 0; i < outer; i++)
            {
                ptr_sp[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());

                Buffer.BlockCopy(spectrogram, i * inner * sizeof(double), tmp_arr, 0, inner * sizeof(double));
                Marshal.Copy(tmp_arr, 0, ptr_sp[i], inner);
            }

            ToolsDefinitions.WriteSpectralEnvelope(filename, fs, f0_length,
                frame_period, fft_size, number_of_dimensions, ptr_sp);

            for (var i = 0; i < spectrogram.GetLength(0); i++)
            {
                Marshal.FreeHGlobal(ptr_sp[i]);
            }
        }

        public static int ReadSpectralEnvelope(string filename, double[,] spectrogram)
        {
            int outer = spectrogram.GetLength(0);
            int inner = spectrogram.GetLength(1);

            IntPtr[] ptrs_sp = new IntPtr[outer];

            for (var i = 0; i < outer; i++)
            {
                ptrs_sp[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());
            }

            var result = ToolsDefinitions.ReadSpectralEnvelope(filename, ptrs_sp);

            var tmp_arr = new double[inner];
            
            for (var i = 0; i < spectrogram.GetLength(0); i++)
            {
                Marshal.Copy(ptrs_sp[i], tmp_arr, 0, inner);
                Buffer.BlockCopy(tmp_arr, 0, spectrogram, i * inner * sizeof(double), inner * sizeof(double));
                Marshal.FreeHGlobal(ptrs_sp[i]);
            }

            return result;
        }

        public static void WriteAperiodicity(string filename, int fs, int f0_length,
            double frame_period, int fft_size, int number_of_dimensions, double[,] aperiodicity)
        {
            int outer = aperiodicity.GetLength(0);
            int inner = aperiodicity.GetLength(1);

            IntPtr[] ptr_ap = new IntPtr[outer];

            var tmp_arr = new double[inner];

            for (var i = 0; i < outer; i++)
            {
                ptr_ap[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());

                Buffer.BlockCopy(aperiodicity, i * inner * sizeof(double), tmp_arr, 0, inner * sizeof(double));
                Marshal.Copy(tmp_arr, 0, ptr_ap[i], inner);
            }

            ToolsDefinitions.WriteAperiodicity(filename, fs, f0_length,
                frame_period, fft_size, number_of_dimensions, ptr_ap);

            for (var i = 0; i < aperiodicity.GetLength(0); i++)
            {
                Marshal.FreeHGlobal(ptr_ap[i]);
            }
        }

        public static int ReadAperiodicity(string filename, double[,] aperiodicity)
        {
            int outer = aperiodicity.GetLength(0);
            int inner = aperiodicity.GetLength(1);

            IntPtr[] ptrs_ap = new IntPtr[outer];

            for (var i = 0; i < outer; i++)
            {
                ptrs_ap[i] = Marshal.AllocHGlobal(inner * Marshal.SizeOf<double>());
            }

            var result = ToolsDefinitions.ReadAperiodicity(filename, ptrs_ap);

            var tmp_arr = new double[inner];
            
            for (var i = 0; i < aperiodicity.GetLength(0); i++)
            {
                Marshal.Copy(ptrs_ap[i], tmp_arr, 0, inner);
                Buffer.BlockCopy(tmp_arr, 0, aperiodicity, i * inner * sizeof(double), inner * sizeof(double));
                Marshal.FreeHGlobal(ptrs_ap[i]);
            }

            return result;
        }
    }
}
