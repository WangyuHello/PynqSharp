using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PynqSharp.Util
{
    public class Mmap
    {
        private const string LibraryName = "libc";

        [Flags]
        public enum FileOpenFlags
        {
            O_RDWR = 0x02,
            O_SYNC = 0x101000
        }

        [DllImport(LibraryName, SetLastError = true)]
        public static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);

        [DllImport(LibraryName, SetLastError = true)]
        public static extern int close(int fd);

        [Flags]
        public enum MemoryMappedProtections
        {
            PROT_NONE = 0x0,
            PROT_READ = 0x1,
            PROT_WRITE = 0x2,
            PROT_EXEC = 0x4
        }

        [Flags]
        public enum MemoryMappedFlags
        {
            MAP_SHARED = 0x01,
            MAP_PRIVATE = 0x02,
            MAP_FIXED = 0x10
        }

        [DllImport(LibraryName, SetLastError = true)]
        public static extern IntPtr mmap(IntPtr addr, int length, MemoryMappedProtections prot, MemoryMappedFlags flags, int fd, int offset);

        [DllImport(LibraryName, SetLastError = true)]
        public static extern int munmap(IntPtr addr, int length);


    }
}
