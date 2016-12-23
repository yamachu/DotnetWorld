using System;

namespace DotnetWorld.API.Common
{
    public interface ITools
    {
        void WavWrite(double[] x, int x_length, int fs, int nbit,
            string filename);

        int GetAudioLength(string filename);

        void WavRead(string filename, out int fs, out int nbit, double[] x);
    }
}