using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace DotnetWorld.API
{
    public partial class Tools
    {
        #if !NETSTANDARD1_2
        public static int GetRawAudioLength(string filename, int nbit)
        {
            if (nbit != 8 && nbit != 16) {
                // Support only 8bit, 16bit
                return -1;
            }

            try 
            {
                using (var fileStream = new FileStream(filename, FileMode.Open))
                {
                    var fileSize = fileStream.Length;

                    return (int)(fileSize / (nbit / 8));
                }
            }
            catch (IOException ex)
            {
                return 0;
            }
        }
        public static void RawRead(string filename, int nbit, double[] x)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                var bytes = binaryReader.ReadBytes(x.Length << (nbit / 8 - 1));
                var zero_line = System.Math.Pow(2.0, nbit - 1);
                var tmp = bytes.Chunks(nbit / 8)
                .Select(b => (double)BitConverter.ToInt16(b.ToArray(), 0) / zero_line)
                .ToArray();

                Array.Copy(tmp, x, tmp.Length);
            }
        }
        #endif
    }

    internal static class LinqExtensions
    {
        // http://stackoverflow.com/a/6362642
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> list, int size)
        {
            while (list.Any())
            {
                yield return list.Take(size);
                list = list.Skip(size);
            }
        }
    }
}