using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpCodesDllsLibrary
{
    public class OpCodesDllHelper
    {
        public static byte[] I2CWrite(byte address, byte writeByte) => new byte[] {25, address, writeByte};
        public static byte[] I2CWrite(byte address, byte register, byte writeByte) => new byte[] {26, address, register, writeByte};
        public static byte[] I2CRead(byte address, byte howManyBytes) => new byte[] {20, address, howManyBytes};
        public static byte[] I2CRead(byte address, byte register, byte howManyBytes) => new byte[] {21, address, register,howManyBytes};

        public static byte[] SoftDelay(ushort ms)
        {
            var lsb = (byte)(ms - (ms << 8));
            var msb = (byte) (ms >> 8);

            return new byte[] {9,msb,lsb};
        }

        //public static byte[] SendBufferLora() => new byte[] {230, 60}; // 230 = log to serial
        public static byte[] SendBuffer() => new byte[] {60}; // 230 = log to serial

        //public static byte[] SendBufferWithCounter() => new byte[] {63};
        public static byte[] ResetCounter() => new byte[] { 11,0,0};

    }
}
