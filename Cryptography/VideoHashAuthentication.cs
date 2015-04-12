using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class VideoHashAuthentication : IExecutable
    {
        private string referenceVideoPath = "6 - 2 - Generic birthday attack (16 min).mp4";
        private string targetVideoPath = "6 - 1 - Introduction (11 min).mp4";
        private string referencedVideoHash = "03c08f4ee0b576fe319338139c045c89c3e8e9409633bea29442e21425006ea8";

        public void Execute()
        {
            byte[] file1Bytes = File.ReadAllBytes(referenceVideoPath);
            string hash = ComputeVideoHash(file1Bytes);
            Console.WriteLine(referencedVideoHash);
            Console.WriteLine(hash);

            byte[] file2Bytes = File.ReadAllBytes(targetVideoPath);
            string hash2 = ComputeVideoHash(file2Bytes);
            Console.WriteLine(hash2);
        }

        private string ComputeVideoHash(byte[] file1Bytes)
        {
            var sha256 = new SHA256Managed();
            var totalChunks = (file1Bytes.Length / 1024);
            byte[] hash = new byte[0];

            for (var j = totalChunks; j >= 0; j--)
            {
                var block = file1Bytes.Skip(j * 1024).Take(1024).ToArray();

                var buffer = new byte[block.Length + hash.Length];
                block.CopyTo(buffer, 0);
                hash.CopyTo(buffer, block.Length);
                hash = sha256.ComputeHash(buffer);
                Trace.WriteLine(j);
            }
            
            return CryptoUtils.ByteArrayToString(hash);
        }
    }
}
