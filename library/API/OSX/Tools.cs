using System;
using System.Runtime.InteropServices;
using DotnetWorld.API.Common;

namespace DotnetWorld.API.OSX
{
    public class Tools : ITools
    {
        public int GetAudioLength(string filename)
            => ToolsDefinitions.GetAudioLength(filename);
            
        public void WavRead(string filename, out int fs, out int nbit, double[] x)
        {
            IntPtr ptr_x = Marshal.AllocHGlobal(Marshal.SizeOf<double>() * x.Length);
            ToolsDefinitions.wavread(filename, out fs, out nbit, ptr_x);
            Marshal.Copy(ptr_x, x, 0, x.Length);
            Marshal.FreeHGlobal(ptr_x);
        }

        public void WavWrite(double[] x, int x_length, int fs, int nbit, string filename)
            => ToolsDefinitions.wavwrite(x, x_length, fs, nbit, filename);
    }
}
