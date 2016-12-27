using System;
using DotnetWorld.API.Common.Struct;
using DotnetWorld.API.Common;

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
            System.Console.WriteLine($"Lenght {(double)(x_length / fs)} [sec]");
        }

        public void F0EstimationDio(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new DioOption();
            var apis = Manager.GetWorldCoreAPI();

            apis.InitializeDioOption(option);

            option.frame_period = world_parameters.frame_period;
            option.speed = 1;
            option.f0_floor = 71.0;
            option.allowed_range = 0.1;

            world_parameters.f0_length = apis.GetSamplesForDIO(world_parameters.fs,
                x_length, world_parameters.frame_period);
            world_parameters.f0 = new double[world_parameters.f0_length];
            world_parameters.time_axis = new double[world_parameters.f0_length];
            double[] refined_f0 = new double[world_parameters.f0_length];

            System.Console.WriteLine("Analysis");
            apis.Dio(x, x_length, world_parameters.fs, option, world_parameters.time_axis,
                world_parameters.f0);
            
            apis.StoneMask(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length, refined_f0);

            for (var i = 0; i < world_parameters.f0_length; ++i)
                world_parameters.f0[i] = refined_f0[i];
        }

        public void F0EstimationHarvest(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new HarvestOption();
            var apis = Manager.GetWorldCoreAPI();

            apis.InitializeHarvestOption(option);

            option.frame_period = world_parameters.frame_period;
            option.f0_floor = 71.0;

            world_parameters.f0_length = apis.GetSamplesForDIO(world_parameters.fs,
                x_length, world_parameters.frame_period);
            world_parameters.f0 = new double[world_parameters.f0_length];
            world_parameters.time_axis = new double[world_parameters.f0_length];
            
            System.Console.WriteLine("Analysis");
            apis.Harvest(x, x_length, world_parameters.fs, option,
                world_parameters.time_axis, world_parameters.f0);
        }

        public void SpectralEnvelopeEstimation(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new CheapTrickOption();
            var apis = Manager.GetWorldCoreAPI();

            apis.InitializeCheapTrickOption(option);

            option.q1 = -0.15;
            option.f0_floor = 71.0;

            world_parameters.fft_size = apis.GetFFTSizeForCheapTrick(world_parameters.fs, option);
            world_parameters.spectrogram = new double[world_parameters.f0_length, world_parameters.fft_size / 2 + 1];
            
            apis.CheapTrick(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length, option,
                world_parameters.spectrogram);
        }

        public void AperiodicityEstimation(double[] x, int x_length, WorldParameters world_parameters)
        {
            var option = new D4COption();
            var apis = Manager.GetWorldCoreAPI();

            apis.InitializeD4COption(option);
            option.threshold = 0.85;

            world_parameters.aperiodicity = new double[world_parameters.f0_length, world_parameters.fft_size / 2 + 1];

            apis.D4C(x, x_length, world_parameters.fs, world_parameters.time_axis,
                world_parameters.f0, world_parameters.f0_length,
                world_parameters.fft_size, option, world_parameters.aperiodicity);
        }

        public void WaveformSynthesis(WorldParameters world_parameters, int fs, int y_length, double[] y)
        {
            var apis = Manager.GetWorldCoreAPI();
            apis.Synthesis(world_parameters.f0, world_parameters.f0_length,
                world_parameters.spectrogram, world_parameters.aperiodicity,
                world_parameters.fft_size, world_parameters.frame_period, fs,
                y_length, y);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2) {
                System.Console.WriteLine($"./app infile outfile");
                // return;
            }

            var world = new WorldSample();
            var apis = DotnetWorld.API.Common.Manager.GetWorldCoreAPI();
            var tools = DotnetWorld.API.Common.Manager.GetWorldToolsAPI();
            var filename = args.Length != 2 ? "/Users/yamachu/Project/CPP/World/test/vaiueo2d.wav" : args[0];
            var x_length = tools.GetAudioLength(filename);

            double[] x = new double[x_length];
            int fs, nbit;
            tools.WavRead(filename, out fs, out nbit, x);

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
            tools.WavWrite(y, y_length, fs, nbit, args.Length != 2 ? "/Users/yamachu/Desktop/resyn_harvest.wav": args[1]);
        }
    }
}
