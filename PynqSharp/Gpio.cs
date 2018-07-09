using System;
using System.IO;
using System.Linq;

namespace PynqSharp
{
    class Gpio
    {
        private const int GpioMinUserPin = 54;

        public int GpioIndex { get; set; }
        public string Direction { get; set; }
        public string Path { get; set; }

        public Gpio(int gpioIndex, string direction)
        {
            GpioIndex = gpioIndex;
            Direction = direction;
            Path = $"/sys/class/gpio/gpio{GpioIndex}/";

            if (!File.Exists(Path))
            {
                using (var f = new StreamWriter(File.OpenWrite("/sys/class/gpio/export")))
                {
                    f.Write($"{GpioIndex}");
                }
            }

            using (var f = new StreamWriter(File.OpenWrite(Path + "direction")))
            {
                f.Write(Direction);
            }
        }

        public int Read()
        {
            if (Direction != "in")
            {
                throw new ArgumentException("Cannot read GPIO output.");
            }

            using (var f = new StreamReader(File.OpenWrite(Path + "value")))
            {
                return f.Read();
            }
        }

        public void Write(int value)
        {
            if (Direction != "out")
            {
                throw new ArgumentException("Cannot write GPIO input.");
            }

            using (var f = new StreamWriter(File.OpenWrite(Path + "value")))
            {
                f.Write($"{value & 1}");
            }
        }

        public static int GetGpioBase()
        {
            var di = new DirectoryInfo("/sys/class/gpio");
            foreach (var dir in di.GetDirectories())
            {
                if (!dir.Name.Contains("gpiochip")) continue;
                var ds = dir.Name.Where(x => x >= '0' && x <= '9');
                var dig = string.Join("", ds);
                return Convert.ToInt32(dig);
            }

            return -1;
        }

        public static int GetGpioPin(int gpioUserIndex)
        {
            return GetGpioBase()+GpioMinUserPin + gpioUserIndex;
        }
    }
}
