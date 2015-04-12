using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class CollisionFinder : IExecutable
    {
        public void Execute()
        {
            Problem1();
            Console.WriteLine("===============================");
            Problem2();
        }

        //f1 = AES(y,x) xor y
        public void Problem1()
        {
            var x2 = CryptoUtils.RandomHex(32);
            var y1 = CryptoUtils.RandomHex(32);
            var y2 = CryptoUtils.RandomHex(32);

            var aes = Aes(y2, x2);
            Func<string, string, string> xor = CryptoUtils.XorHexed;

            var xored = xor(xor(aes, y2).ToLower(), y1).ToLower();

            var x1 = AesInv(y1, xored);

            Console.WriteLine("x1: " + x1);
            Console.WriteLine("x2: " + x2);
            Console.WriteLine("y1: " + y1);
            Console.WriteLine("y2: " + y2);
            Console.WriteLine("AES(y1,x1) xor y1: " + xor(Aes(y1,x1), y1));
            Console.WriteLine("AES(y2,x2) xor y2: " + xor(Aes(y2, x2), y2));
        }

        //f2 = AES(x,x) xor y
        public void Problem2()
        {
            var x2 = CryptoUtils.RandomHex(32);
            var x1 = CryptoUtils.RandomHex(32);
            var y2 = CryptoUtils.RandomHex(32);

            var aes1 = Aes(x1, x1);
            var aes2 = Aes(x2, x2);
            Func<string, string, string> xor = CryptoUtils.XorHexed;

            var y1 = xor(xor(aes1, aes2).ToLower(), y2).ToLower();

            Console.WriteLine("x1: " + x1);
            Console.WriteLine("x2: " + x2);
            Console.WriteLine("y1: " + y1);
            Console.WriteLine("y2: " + y2);
            Console.WriteLine("AES(y1,x1) xor y1: " + xor(Aes(x1, x1), y1));
            Console.WriteLine("AES(y2,x2) xor y2: " + xor(Aes(x2, x2), y2));
        }

        public string Aes(string key, string message)
        {
            var keyBytes = CryptoUtils.StringToByteArray(key);
            var messageBytes = CryptoUtils.StringToByteArray(message);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.KeySize = 128;
            cipher.BlockSize = 128;
            cipher.Mode = CipherMode.ECB;
            cipher.Padding = PaddingMode.None;

            cipher.Key = keyBytes;

            var decryptor = cipher.CreateEncryptor();

            byte[] result;
            var decodedMessage = decryptor.TransformFinalBlock(messageBytes, 0, messageBytes.Length);

            return CryptoUtils.ByteArrayToString(decodedMessage);
        }

        public string AesInv(string key, string ciphertext)
        {
            var keyBytes = CryptoUtils.StringToByteArray(key);
            var cipherMsgBytes = CryptoUtils.StringToByteArray(ciphertext);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.KeySize = 128;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.None;
            cipher.Mode = CipherMode.ECB;

            cipher.Key = keyBytes;

            var decryptor = cipher.CreateDecryptor();

            byte[] result;
            var decodedMessage = decryptor.TransformFinalBlock(cipherMsgBytes, 0, cipherMsgBytes.Length);

            return CryptoUtils.ByteArrayToString(decodedMessage).Substring(0, 32);
        }
    }
}
