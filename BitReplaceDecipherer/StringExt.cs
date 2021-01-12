using System;
using System.Linq;

namespace BitReplaceDecipherer
{
    public static class StringExt
    {
        public static byte[] StringToBytes(this string str)
        {
            var bytes = new byte[0];
            return str.Aggregate(bytes, (current, symbol) => current.Concat(BitConverter.GetBytes(symbol)).ToArray());
        }
    }
}