using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cryptography
{
    public class CribDrag : IExecutable
    {
        private bool shift = true;
        private string target = "32510ba9babebbbefd001547a810e67149caee11d945cd7fc81a05e9f85aac650e9052ba6a8cd8257bf14d13e6f0a803b54fde9e77472dbff89d71b57bddef121336cb85ccb8f3315f4b52e301d16e9f52f904";

        private List<string> GetPopularPhrases()
        {
            return File.ReadAllLines("popular.txt").ToList();
        }

        private List<string> GetCiphers()
        {
            return File.ReadAllLines("ciphers.txt").ToList();
        }

        public void Execute()
        {
            var phrases = GetPopularPhrases();
            phrases = new List<string>() { " the " };
            var length = phrases.Count();
            
            var ciphers = GetCiphers();
            List<WordMatch> matches = new List<WordMatch>();

            for(var i = 0; i < ciphers.Count(); i++)           
            {
                Debug.WriteLine("\n" + i + "\n");
                var xor = XorStrings(target, ciphers[i]);
                for (var j = 0; j < phrases.Count(); j++)
                {
                    var phraseHex = CryptoUtils.StringToHex(phrases[j]);
                    var timesInXor = (xor.Length/2 - phraseHex.Length/2) + 1;

                    for(var k = 0; k < timesInXor; k+=2)
                    {
                        var result = XorStrings(xor.Substring(k*2, phraseHex.Length), phraseHex);
                        var resultAscii = CryptoUtils.HexToString(result);

                        Debug.WriteLine(resultAscii);

                        for (var q = 0; q < phrases[j].Length; q++)
                        {
                            if(!Char.IsWhiteSpace(phrases[j][q]))
                            {
                                if(!(Char.IsLetter(resultAscii[q]) || Char.IsWhiteSpace(resultAscii[q])))
                                {
                                    Debug.WriteLine(resultAscii);
                                }
                            }
                        }
                    }
                }                
            }
        }

        class WordMatch
        {
            public int Position { get; set; }
            public string Word { get; set; }
            public int Count { get; set; }
            public string FoundWord { get; set; }
        }

        private string XorStrings(string first, string second)
        {
            var length = Math.Min(first.Length, second.Length);

            var f = first.Substring(0, length);
            var s = second.Substring(0, length);

            return CryptoUtils.XorHexed(f, s);
        }
    }
}
