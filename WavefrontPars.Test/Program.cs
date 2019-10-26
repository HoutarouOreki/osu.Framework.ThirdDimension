using System;
using System.IO;

namespace WavefrontPars.Test
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No file specified to test");
                return;
            }
            var fileInfo = new FileInfo(args[0]);
            if (!fileInfo.Exists)
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            foreach (var obj in Parser.ParseText(File.ReadAllText(args[0])))
            {
                Console.WriteLine(obj.Name);
                foreach (var triangle in obj.Triangles)
                    Console.WriteLine($"{triangle.P1}, {triangle.P2}, {triangle.P3}");
                Console.WriteLine();
            }
        }
    }
}
