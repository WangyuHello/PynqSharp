using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace PynqSharp
{
    public class Xlnk : IDisposable
    {
        private static uint xlnkBufCnt = 0;

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong cma_mmap(uint phyAddr, uint len);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong cma_munmap(IntPtr buf, uint len);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr cma_alloc(uint len, uint cacheable);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong cma_get_phy_addr(IntPtr buf);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern void cma_free(IntPtr buf);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint cma_pages_available();

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern void cma_flush_cache(IntPtr buf, uint phys_addr, int size);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern void cma_invalidate_cache(IntPtr buf, uint phys_addr, int size);

        [DllImport("/usr/lib/libsds_lib.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _xlnk_reset();

        public void Dispose()
        {
            
        }

        public Dictionary<IntPtr,uint> BufMap { get; set; }

        public unsafe Span<uint> cma_alloc_(uint length, uint cacheable, string data_type = "void")
        {
            if (data_type != "void")
            {

            }

            var buf = cma_alloc(length, cacheable);

            if (buf == IntPtr.Zero)
            {
                throw new Exception("Failed to allocate Memory!");
            }

            BufMap.Add(buf, length);

            return new Span<uint>(buf.ToPointer(), (int)length);
        }

        public ulong cma_get_phy_addr_(IntPtr buf_ptr)
        {
            return cma_get_phy_addr(buf_ptr);
        }

        public unsafe static void cma_memcopy(IntPtr dest, IntPtr src, int nbytes)
        {
            var s1 = new Span<uint>(dest.ToPointer(), nbytes);
            var s2 = new Span<uint>(src.ToPointer(), nbytes);

            s2.CopyTo(s1);
        }

        public void cma_free_(IntPtr buf)
        {
            if (BufMap.ContainsKey(buf))
            {
                BufMap.Remove(buf);
            }

            cma_free(buf);
        }

        public void xlnk_reset()
        {
            BufMap.Clear();
            _xlnk_reset();
        }
    }

    public class ContiguousArray
    {

    }

}
