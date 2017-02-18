using System;
using DotnetWorld.API.Structs;
using DotnetWorld.API;
using System.Runtime.InteropServices;

namespace ConsoleApplication
{
    public class WorldParameters
    {
        public double frame_period;
        public int fs;

        public double[] f0;
        public double[] time_axis;
        public int f0_length;

        public double[,] spectrogram;
        public double[,] aperiodicity;
        public int fft_size;
    }

    public class WorldSample
    {
        public void DisplayInformation(int fs, int nbit, int x_length)
        {
            System.Console.WriteLine("File information");
            System.Console.WriteLine($"Sampling : {fs} Hz {nbit} Bit");
            System.Console.WriteLine($"Length {x_length} [sample]");
            System.Console.WriteLine($"Lenght {((double)x_length / fs)} [sec]");
        }

        public void F0EstimationDio(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new DioOption();
            
            Core.InitializeDioOption(option);

            option.frame_period = world_parameters.frame_period;
            option.speed = 1;
            option.f0_floor = 71.0;
            option.allowed_range = 0.1;

            world_parameters.f0_length = Core.GetSamplesForDIO(world_parameters.fs,
                x_length, world_parameters.frame_period);
            world_parameters.f0 = new double[world_parameters.f0_length];
            world_parameters.time_axis = new double[world_parameters.f0_length];
            double[] refined_f0 = new double[world_parameters.f0_length];

            System.Console.WriteLine("Analysis");
            Core.Dio(x, x_length, world_parameters.fs, option, world_parameters.time_axis,
                world_parameters.f0);
            
            Core.StoneMask(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length, refined_f0);

            for (var i = 0; i < world_parameters.f0_length; ++i)
                world_parameters.f0[i] = refined_f0[i];
        }

        public void F0EstimationHarvest(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new HarvestOption();

            Core.InitializeHarvestOption(option);
            
            option.frame_period = world_parameters.frame_period;
            option.f0_floor = 71.0;

            world_parameters.f0_length = Core.GetSamplesForDIO(world_parameters.fs,
                x_length, world_parameters.frame_period);
            world_parameters.f0 = new double[world_parameters.f0_length];
            world_parameters.time_axis = new double[world_parameters.f0_length];
            
            System.Console.WriteLine("Analysis");
            Core.Harvest(x, x_length, world_parameters.fs, option,
                world_parameters.time_axis, world_parameters.f0);
        }

        public void SpectralEnvelopeEstimation(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new CheapTrickOption();

            Core.InitializeCheapTrickOption(world_parameters.fs, option);

            option.q1 = -0.15;
            option.f0_floor = 71.0;

            world_parameters.fft_size = Core.GetFFTSizeForCheapTrick(world_parameters.fs, option);
            world_parameters.spectrogram = new double[world_parameters.f0_length, world_parameters.fft_size / 2 + 1];
            
            Core.CheapTrick(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length, option,
                world_parameters.spectrogram);
        }

        public void AperiodicityEstimation(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new D4COption();

            Core.InitializeD4COption(option);
            option.threshold = 0.85;

            world_parameters.aperiodicity = new double[world_parameters.f0_length, world_parameters.fft_size / 2 + 1];

            Core.D4C(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length,
                world_parameters.fft_size, option, world_parameters.aperiodicity);
        }

        public void WaveformSynthesis(WorldParameters world_parameters, int fs, int y_length, double[] y)
        {
            Core.Synthesis(world_parameters.f0, world_parameters.f0_length,
                world_parameters.spectrogram, world_parameters.aperiodicity,
                world_parameters.fft_size, world_parameters.frame_period, fs,
                y_length, y);
        }

        public void WaveformSynthesis2(WorldParameters world_parameters, int fs, int y_length, double[] y)
        {            
            var synthesizer = new WorldSynthesizer();
            int buffer_size = 64;
            Core.InitializeSynthesizer(world_parameters.fs, world_parameters.frame_period,
                world_parameters.fft_size, buffer_size, 100, synthesizer);

            Core.AddParameters(world_parameters.f0, world_parameters.f0_length,
                world_parameters.spectrogram, world_parameters.aperiodicity,
                synthesizer);

            int index;
            var _buf = new double[buffer_size];

            for (var i = 0; Core.Synthesis2(synthesizer); ++i)
            {
                index = i * buffer_size;
                synthesizer.CopyFromBufferToArray(_buf);
                for (var j = 0; j < buffer_size; ++j)
                    y [j + index] = _buf[j];
            }

            Core.DestroySynthesizer(synthesizer);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2) {
                System.Console.WriteLine($"./app infile outfile");
                // return;
            }

            var world = new WorldSample();

            var filename = args.Length < 1 ? "/Users/yamachu/Project/CPP/World/test/vaiueo2d.wav" : args[0];
            var x_length = Tools.GetAudioLength(filename);
            System.Console.WriteLine(x_length);

            double[] x = new double[x_length];
            int fs, nbit;
            Tools.WavRead(filename, out fs, out nbit, x);

            world.DisplayInformation(fs, nbit, x_length);

            var parameters = new WorldParameters();
            parameters.fs = fs;
            parameters.frame_period = 5.0;

            // world.F0EstimationDio(x, x_length, parameters);
            world.F0EstimationHarvest(x, x_length, parameters);

            world.SpectralEnvelopeEstimation(x, x_length, parameters);

            world.AperiodicityEstimation(x, x_length, parameters);

            int y_length = (int)((parameters.f0_length - 1) * parameters.frame_period / 1000.0 * fs) + 1;
            double[] y = new double[y_length];
            for (var i = 0; i < y.Length; i++)
                y[i] = 0.0;

            world.WaveformSynthesis(parameters, fs, y_length, y);
            Tools.WavWrite(y, y_length, fs, nbit, args.Length < 2 ? "/Users/yamachu/Desktop/resyn_harvest_normal.wav": args[1]);

            for (var i = 0; i < y.Length; i++)
                y[i] = 0.0;

            if (args.Length <= 1 || args.Length > 2)
            {
                world.WaveformSynthesis2(parameters, fs, y_length, y);
                Tools.WavWrite(y, y_length, fs, nbit, args.Length < 3 ? "/Users/yamachu/Desktop/resyn_harvest_realtime.wav": args[2]);
            }
        }
    }
}
