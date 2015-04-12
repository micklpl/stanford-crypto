using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public static class CryptoUtils
    {
        public static Random random = new Random();

        public static string XorHexed(string message, string cipher)
        {
            var baMessage = StringToByteArray(message);
            var baCipher = StringToByteArray(cipher);

            return exclusiveOR(baMessage, baCipher);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static string exclusiveOR(byte[] key, byte[] PAN)
        {
            if (key.Length == PAN.Length)
            {
                byte[] result = new byte[key.Length];
                for (int i = 0; i < key.Length; i++)
                {
                    result[i] = (byte)(key[i] ^ PAN[i]);
                }
                string hex = BitConverter.ToString(result).Replace("-", "");
                return hex;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static string RandomHex(int length)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var number = random.Next() % 16;
                sb.Append(number.ToString("x"));
            }
            return sb.ToString();
        }

        public static string StringToHex(string str)
        {
            byte[] ba = Encoding.Default.GetBytes(str);
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "");
        }

        public static string HexToString(string hexString)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return sb.ToString();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
