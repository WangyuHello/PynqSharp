using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using PynqSharp.Util;

namespace PynqSharp
{
    class Mmio
    {
        public int VirtBase { get; set; }
        public int VirtOffset { get; set; }
        public int BaseAddr { get; set; }
        public int Length { get; set; }
        public int MmapFile { get; set; }   
        public IntPtr Mem { get; set; }

        public Mmio(int baseAddr, int length = 4)
        {
            if (baseAddr < 0 || length < 0 )
            {
                throw new ArgumentException("Base address or length cannot be negative.");
            }

            VirtBase = baseAddr & ~(Environment.SystemPageSize - 1);
            VirtOffset = baseAddr - VirtBase;

            BaseAddr = baseAddr;
            Length = length;

            Debug.WriteLine($"MMIO(address, size) = ({BaseAddr:x}, {Length:x} bytes)");

            MmapFile = Mmap.open("/dev/mem", Mmap.FileOpenFlags.O_RDWR | Mmap.FileOpenFlags.O_SYNC);

            Mem = Mmap.mmap(IntPtr.Zero, Length + VirtOffset,
                Mmap.MemoryMappedProtections.PROT_READ | Mmap.MemoryMappedProtections.PROT_WRITE,
                Mmap.MemoryMappedFlags.MAP_SHARED, MmapFile, VirtBase);
        }

        public int Read(int offset = 0, int length = 4)
        {
            if (length != 4)
            {
                throw new ArgumentException("MMIO currently only supports 4-byte reads.");
            }

            if (offset < 0)
            {
                throw new ArgumentException("Offset cannot be negative.");
            }

            if ((offset % 4) != 0)
            {
                throw new ArgumentException("Unaligned read: offset must be multiple of 4.");
            }

            Debug.WriteLine($"Reading {length} bytes from offset {offset:x}");

            return Marshal.ReadInt32(Mem, offset);
        }

        public void Write(int offset, int data)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset cannot be negative.");
            }

            if ((offset % 4) != 0)
            {
                throw new ArgumentException("Unaligned read: offset must be multiple of 4.");
            }

            Marshal.WriteInt32(Mem, offset, data);
        }

        public void Write(int offset, byte[] data)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset cannot be negative.");
            }

            if ((offset % 4) != 0)
            {
                throw new ArgumentException("Unaligned read: offset must be multiple of 4.");
            }

            var start = IntPtr.Add(Mem, offset);
            Marshal.Copy(data, 0, start, data.Length);
        }
    }
}
