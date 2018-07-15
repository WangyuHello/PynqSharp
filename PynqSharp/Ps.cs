using System;
using System.Collections.Generic;
using System.Text;


namespace PynqSharp
{
    public class Ps
    {
        //Pynq Family Constants
        public const string ZYNQ_ARCH = "armv7l";

        //Clock constants
        public const double SRC_CLK_MHZ = 50.0;
        public const double DEFAULT_CLK_MHZ = 100.0;

        public const uint SCLR_BASE_ADDRESS = 0xf8000000;
        public const uint ARM_PLL_DIV_OFFSET = 0x100;
        public const uint DDR_PLL_DIV_OFFSET = 0x104;
        public const uint IO_PLL_DIV_OFFSET = 0x108;
        public const uint ARM_CLK_REG_OFFSET = 0x120;
        public uint[] CLK_CTRL_REG_OFFSET = new uint[] { 0x170, 0x180, 0x190, 0x1A0 };
        public const uint PLL_DIV_LSB = 12;
        public const uint PLL_DIV_MSB = 18;
        public const uint ARM_CLK_SEL_LSB = 4;
        public const uint ARM_CLK_SEL_MSB = 5;
        public const uint ARM_CLK_DIV_LSB = 8;
        public const uint ARM_CLK_DIV_MSB = 13;
        public const uint CLK_SRC_LSB = 4;
        public const uint CLK_SRC_MSB = 5;
        public const uint CLK_DIV0_LSB = 8;
        public const uint CLK_DIV0_MSB = 13;
        public const uint CLK_DIV1_LSB = 20;
        public const uint CLK_DIV1_MSB = 25;

    //    VALID_CLOCK_DIV_PRODUCTS = sorted(list(set(
    //(np.multiply(
    //    np.arange(1 << (CLK_DIV1_MSB - CLK_DIV1_LSB + 1)).reshape(
    //        1 << (CLK_DIV1_MSB - CLK_DIV1_LSB + 1), 1),
    //    np.arange(1 << (CLK_DIV0_MSB - CLK_DIV0_LSB + 1)))).reshape(-1))))

        public static (int,int) Get2Divisors(double freqHigh, double freqDesired, uint reg0Width, uint reg1With)
        {
            var div_product_desired = Math.Round(freqHigh / freqDesired, 6);

            return (0, 0);
        }
    }

    public class Register
    {
        public int Address { get; set; }
        public int Width { get; set; }
        public Memory<uint> _Buffer { get; set; }

        public Register(int address, int width = 32)
        {
            Address = address;
            Width = width;

            if (width == 32)
            {
                _Buffer = new Mmio(address)._Array;
            }
            else if (width == 64)
            {

            }
            else
            {

            }
        }

        public long this[int index]
        {
            get
            {
                var curr_val = _Buffer.Span[0];
                var mask = 1 << index;
                return (curr_val & mask) >> index;
            }
            set
            {
                var curr_val = _Buffer.Span[0];
                if (value !=0 && value != 1)
                {
                    throw new Exception("Value to be set should be either 0 or 1.");
                }

                var mask = 1 << index;
                _Buffer.Span[0] = (uint)((curr_val & ~mask) | (value << index));
            }
        }

        public override string ToString()
        {
            return $"{_Buffer.Span[0]:x}";
        }
    }
}
