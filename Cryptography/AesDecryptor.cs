using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography;
using System.Security.Cryptography;
using System.IO;

namespace Cryptography
{
    public class AesDecryptor : IExecutable
    {
        public void Execute()
        {
            var keysAndCiphers = File.ReadAllLines("SymmetricMessages.txt");
            for (var i = 0; i < 4; i+=2)
            {
                DecryptCbcWithPkcs5(keysAndCiphers[i], keysAndCiphers[i+1]);
                DecryptCbcWithPkcs5Method2(keysAndCiphers[i], keysAndCiphers[i + 1]);
            }
            for (var i = 4; i < 8; i += 2)
            {
                DecryptCtr(keysAndCiphers[i], keysAndCiphers[i + 1]);
            } 
        }

        private void DecryptCbcWithPkcs5(string key, string ciphertext)
        {
            var keyBytes = CryptoUtils.StringToByteArray(key);
            var ivString = ciphertext.Substring(0, 32);
            var cipherTextMsg = ciphertext.Substring(32);
            var cipherMsgBytes = CryptoUtils.StringToByteArray(cipherTextMsg);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.KeySize = 128;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.PKCS7;
            cipher.Mode = CipherMode.CBC;

            cipher.Key = keyBytes;
            cipher.IV = CryptoUtils.StringToByteArray(ivString);

            var decryptor = cipher.CreateDecryptor();

            byte[] result;
            var decodedMessage = decryptor.TransformFinalBlock(cipherMsgBytes, 0, cipherMsgBytes.Length);

            var resultHex = CryptoUtils.ByteArrayToString(decodedMessage);
            var message = CryptoUtils.HexToString(resultHex);

            Console.WriteLine(message);
        }

        private void DecryptCtr(string key, string ciphertext)
        {
            
        }

        public static Byte[] XorArrays(Byte[] a, Byte[] b)
        {
            var res = new List<Byte>();
            int bor = Math.Min(a.Length, b.Length);
            for (int i = 0; i < bor; i++)
                res.Add((byte)(a[i] ^ b[i]));
            return res.ToArray();
        }

        
        private void DecryptCbcWithPkcs5Method2(string key, string ciphertext)
        {
            var keyBytes = CryptoUtils.StringToByteArray(key);
            var ivString = ciphertext.Substring(0, 32);
            var cipherTextMsg = ciphertext.Substring(32);
            var cipherMsgBytes = CryptoUtils.StringToByteArray(cipherTextMsg);

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = CryptoUtils.StringToByteArray(key);
                aes.IV = CryptoUtils.StringToByteArray(ivString);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherMsgBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            Console.WriteLine(srDecrypt.ReadToEnd());
                        }
                    }
                }
            }
        }
    }

}
