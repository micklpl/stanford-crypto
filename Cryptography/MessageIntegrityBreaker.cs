using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class MessageIntegrityBreaker : IExecutable
    {
        public void Execute()
        {
            var originalMessage = "Pay Bob 100$";
            var maliciousMessage = "Pay Bob 500$";
            var interceptedIv = "20814804c1767293b99f1d9cab3bc3e7";

            var msgHex = CryptoUtils.StringToHex(originalMessage).ToLower();
            var destMsgHex = CryptoUtils.StringToHex(maliciousMessage).ToLower();

            for (var i = msgHex.Length; i < interceptedIv.Length; i++)
            {
                msgHex += "0";
                destMsgHex += "0";
            }

            Func<string, string, string> xor = CryptoUtils.XorHexed;

            var newIv = xor(xor(interceptedIv, msgHex), destMsgHex);

            Console.WriteLine(newIv.ToLower());
        }
    }
}
