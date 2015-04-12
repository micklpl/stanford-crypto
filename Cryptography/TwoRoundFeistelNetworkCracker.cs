using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class TwoRoundFeistelNetworkCracker : IExecutable
    {
        public void Execute()
        {
            //1st col - Feistel Network output for 0^64 input
            //2nd col - Feistel Network output for 0^32 || 1^32
            Dictionary<string, string> ciphers = new Dictionary<string, string>()
            {
                {"290b6e3a39155d6f", "d6f491c5b645c008"},
                {"7c2822ebfdc48bfb", "325032a9c5e2364b"},
                {"2d1cfa42c0b1d266", "eea6e3ddb2146dd0"},
                {"4af532671351e2e1", "87a40cfa8dd39154"}
            };

            foreach (var cipherPair in ciphers)
            {
                var xorResult = CryptoUtils.XorHexed(cipherPair.Key, cipherPair.Value);
                var firstHalf = xorResult.Substring(0, xorResult.Length / 2);
                if(firstHalf.All(s => s == 'F'))
                {
                    Console.WriteLine("Feistel Network output is: {0}, {1}",
                        cipherPair.Key, cipherPair.Value);
                }
            }
        }
    }
}
