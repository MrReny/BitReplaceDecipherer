using System;
using System.Linq;
using BitReplaceDecipherer;
namespace EasyDecipherer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var str = "Совершенно секретно блеть";
            var strBytes = str.StringToBytes();

            for (int i = 0; i < strBytes.Length; i++)
            {
                strBytes[i] = strBytes[i].SwitchBits(1,6);
                strBytes[i] = strBytes[i].SwitchBits(2,7);
                strBytes[i] = strBytes[i].SwitchBits(3,4);
                strBytes[i] = strBytes[i].SwitchBits(0,5);
            }

            var decipher =  Decipherer.DecipherWithCommonStarter(strBytes);

             if(decipher == str)
                 Console.WriteLine("Deciphered");
             else
             {
                 Console.WriteLine("Not deciphered");
             }

        }
    }
}