using System;

namespace BitReplaceDecipherer
{
    public static class ByteExt
    {
        public static byte SwitchBits(this byte value, int first, int second)
        {
            int bi, bj; // значения i-го и j-го битов
            bi = (byte)(value >> first) & 1;
            bj = (byte)(value >> second) & 1;
            var uiValue = (uint)value;
            if (bi != bj)
                if (bi == 1) {
                    uiValue -= (uint)1 << first;
                    uiValue += (uint)1 << second;
                } else {
                    uiValue -= (uint)1 << second;
                    uiValue += (uint)1 << first;
                }

            return Convert.ToByte(uiValue);
        }
    }
}