using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace FlushFileBuffersTest
{
    public enum ExtendedFileOptions
    {
        NoBuffering = unchecked((int)0x20000000),
        Overlapped = unchecked((int)0x40000000),
        SequentialScan = unchecked((int)0x08000000),
        WriteThrough = unchecked((int)0x80000000)

    }

    class Program
    {
        static void Main(string[] args)
        {
            var flags = ExtendedFileOptions.NoBuffering;
            flags = flags | ExtendedFileOptions.WriteThrough;
            var handle = CreateFile("a.dat", FileAccess.ReadWrite, FileShare.ReadWrite,
                IntPtr.Zero,
                FileMode.OpenOrCreate,
                (int)flags,
                IntPtr.Zero);

            while (true)
            {
                Console.ReadLine();
                var watch = new Stopwatch();
                watch.Start();
                FlushFileBuffers(handle);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FlushFileBuffers(SafeFileHandle filehandle);

        [DllImport("KERNEL32", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        public static extern SafeFileHandle CreateFile(String fileName,
            FileAccess desiredAccess,
            FileShare shareMode,
            IntPtr securityAttrs,
            FileMode creationDisposition,
            int flagsAndAttributes,
            IntPtr templateFile);
    }
}
