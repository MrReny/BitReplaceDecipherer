using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;

namespace BitReplaceDecipherer
{
    public class Decipherer
    {
        private static readonly string[] CommonStarters = {"Совершенно Секретно"};

        public static string DecipherWithCommonStarter(byte[] cipheredBytes)
        {
            foreach (var commonStarter in CommonStarters)
            {
                var result = DecipherByBitReplace(commonStarter, cipheredBytes);
                if (result != "") return result;
            }
            return "";
        }

        public static string DecipherByBitReplace(string possiblyStartWith, byte[] cipheredBytes)
        {
            byte[] commonBytes = possiblyStartWith.StringToBytes();
            var isDeciphered = false;

            Dictionary<int, int> bytePositions = Enumerable.Range(0, 8).ToDictionary(pos => pos);

            string decipheredText = "";

            while (!isDeciphered)
            {
                foreach (var pare in commonBytes.Zip(cipheredBytes, (f,s)=> (f,s)))
                {
                    var bitsToSkip = bytePositions
                        .Where(kp=> kp.Key != kp.Value)
                        .Select( kp=> kp.Key).ToArray();
                    var bitSign = IsContainsOnlyOneActiveBit(pare.f, out var origIndex, bitsToSkip);
                    if(origIndex == -1) continue;
                    var cBitSign = IsContainsOnlyOneActiveBit(pare.s, out var stepIndex, bitsToSkip);

                    if (bitSign != cBitSign) throw new Exception();

                    if(bytePositions[origIndex] == origIndex && bytePositions[stepIndex] == stepIndex)
                        bytePositions.Swap(origIndex, stepIndex);
                }

                var dict = bytePositions;

                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        dict.Remove(dict[i]);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                decipheredText = TryDecipher(possiblyStartWith, cipheredBytes, dict);
                isDeciphered = decipheredText !="";
            }

            return decipheredText;
        }

        private static string TryDecipher(string commonString, byte[] cipheredBytes, IDictionary<int, int> cipherDict)
        {
            for (int i = 0; i < cipheredBytes.Length; i++)
            {
                foreach (var kp in cipherDict)
                {
                    cipheredBytes[i] = cipheredBytes[i].SwitchBits(kp.Key, kp.Value);
                }
            }

            string result = System.Text.Encoding.Unicode.GetString(cipheredBytes);
            if(commonString.ToLower() == result.Substring(0, commonString.Length).ToLower())
                return result;
            else
            {
                return "";
            }
        }

        private static bool IsContainsOnlyOneActiveBit(byte value, out int index, int[] bitsToSkip = null)
        {
            var countZ = 0;
            var countO = 0;

            var indexZ = 0;
            var indexO = 0;
            for (int i = 0; i < 8; i++)
            {
                if(bitsToSkip != null && bitsToSkip.Any( b=> b == i)) continue;

                if ((value >> i & 1) == 1)
                {
                    countO++;
                    indexO = i;
                }
                else
                {
                    countZ++;
                    indexZ = i;
                }

                if (countO > 1 && countZ > 1)
                {
                    index = -1;
                    return false;
                }
            }

            if (countO == 1)
            {
                index = indexO;
                return true;
            }
            else if(countZ == 1)
            {
                index = indexZ;
                return false;
            }

            index = -1;
            return false;
        }
    }
}