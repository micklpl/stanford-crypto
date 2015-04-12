using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class PaddingOracle : IExecutable
    {

        private string urlPrefix = "http://crypto-class.appspot.com/po?er=";
        private string originalToken = "f20bdba6ff29eed7b046d1df9fb7000058b1ffb4210a580f748b4ac714c001bd4a61044426fb515dad3f21f18aa577c0bdf302936266926ff37dbf7035d5eeb4";

        public void Execute()
        {
            var blocks = new string[4];
            Func<string, string, string> xor = CryptoUtils.XorHexed;
            for (var i = 0; i < 4; i++)
            {
                blocks[i] = new string(originalToken.Skip(i * 32).Take(32).ToArray());
            }

            var guessed = new string[3]; //last block is omitted

            for (var m = 0; m < guessed.Length; m++)
            {
                guessed[m] = string.Empty;
            }

            for (var j = 2; j >= 0; j--)
            {
                string currentBlock = blocks[j];
                for (var k = 0; k < 16; k++)
                {
                    var pad = (k + 1).ToString("x2");
                    var start = 32 - k * 2 - 2;
                    var exploredBytes = currentBlock.Substring(start, 2);
                    for (var l = 0; l < 256; l++)
                    {
                        var guess = l.ToString("x2");
                        var valueToSubstitute = xor(xor(exploredBytes, guess), pad).ToLower();
                        var token = BuildToken(j, k, valueToSubstitute, guessed);
                        bool isCorrect = IsPaddingCorrect(token);

                        if (isCorrect)
                        {
                            guessed[j] = guess + guessed[j];
                            Console.WriteLine(guess);
                            break;
                        }

                        if (!isCorrect && l == 255)
                        {
                            guessed[j] = pad + guessed[j];
                            Console.WriteLine(pad);                            
                        }
                    }
                }
            }

            var str = String.Concat(guessed[0] + guessed[1] + guessed[2]);

            Console.WriteLine("\n\n");
            Console.WriteLine(CryptoUtils.HexToString(str));
        }

        private string BuildToken(int blockIndex, int pad, string newValue, string[] guessed)
        {            
            int startingIndex = blockIndex * 32 + (15*2 - pad*2);
            var suffix = originalToken.Substring(startingIndex + 2);
            var replacedCurrent = string.Concat(originalToken.Substring(0, startingIndex), newValue, 
                suffix.Substring(0, suffix.Length - (2 - blockIndex)*32) );
            var currentGuessed = guessed[blockIndex];
            if (currentGuessed == string.Empty)
                return replacedCurrent;
            for (var i = 0; i < currentGuessed.Length / 2; i++)
            {
                var index = (blockIndex + 1) * 32 - (i * 2) - 2;
                var newValue2 =
                    CryptoUtils.XorHexed(originalToken.Substring(index, 2), currentGuessed.Substring(currentGuessed.Length - i * 2 - 2, 2));
                newValue2 = CryptoUtils.XorHexed(newValue2, (pad+1).ToString("x2"));
                replacedCurrent = string.Concat(replacedCurrent.Substring(0, index), newValue2, replacedCurrent.Substring(index + 2));
            }
            return replacedCurrent.ToLower();
        }

        private bool IsPaddingCorrect(string token)
        {
            var url = urlPrefix + token;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse webResponse;
            try
            {
                webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException e)
            {
                if(e.ToString().Contains("404")) return true;
                if (e.ToString().Contains("403")) return false;
                throw;
            }
            return false;
        }
    }
}
