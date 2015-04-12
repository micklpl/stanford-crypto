using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class MessageSubstitution : IExecutable
    {
        public void Execute()
        {
            var plainText = "attack at dawn";

            var messagebyteArray = CryptoUtils.StringToHex(plainText).ToLower();

            var cipherBytes = "6c73d5240a948c86981bc294814d";

            var key = CryptoUtils.XorHexed(messagebyteArray, cipherBytes).ToLower();

            var maliciousText = "attack at dusk";
            var maliciousTextByteArray = CryptoUtils.StringToHex(maliciousText).ToLower();

            var cipheredMaliciousText = CryptoUtils.XorHexed(key, maliciousTextByteArray).ToLower();

            Console.WriteLine(cipheredMaliciousText);
        }        

        public static string exclusiveOR(string string_1, string string_2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < string_1.Length; i++)
                sb.Append((char)(string_1[i] ^ string_2[(i % string_2.Length)]));
            String result = sb.ToString();

            return result;
        }               
    }
}
