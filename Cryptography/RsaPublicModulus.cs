using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace Cryptography
{
    public class RsaPublicModulus : IExecutable
    {
        public void Execute()
        {
            BigInteger N = BigInteger.Parse("179769313486231590772930519078902473361797697894230657273430081157732675805505620686985379449212982959585501387537164015710139858647833778606925583497541085196591615128057575940752635007475935288710823649949940771895617054361149474865046711015101563940680527540071584560878577663743040086340742855278549092581");
            Console.WriteLine("Problem 1:");
            var p = CalculateFactor(N);
            Console.WriteLine(p);

            N = BigInteger.Parse("648455842808071669662824265346772278726343720706976263060439070378797308618081116462714015276061417569195587321840254520655424906719892428844841839353281972988531310511738648965962582821502504990264452100885281673303711142296421027840289307657458645233683357077834689715838646088239640236866252211790085787877");
            Console.WriteLine("Problem 2:");
            Console.WriteLine(CalculateFactor(N));

            N = BigInteger.Parse("720062263747350425279564435525583738338084451473999841826653057981916355690188337790423408664187663938485175264994017897083524079135686877441155132015188279331812309091996246361896836573643119174094961348524639707885238799396839230364676670221627018353299443241192173812729276147530748597302192751375739387929");
            Console.WriteLine("Problem 3:");
            Console.WriteLine(Challenge3(N));

            DecryptRsa(N);
        }      

        private BigInteger CalculateFactor(BigInteger N)
        {
            BigInteger sqrtOfN = CryptoUtils.Sqrt(N);
            int scanRange = (int)Math.Pow(2, 20);
            for (int i = 0; i < scanRange; i++)
            {
                Debug.WriteLine(i);
                BigInteger x;

                BigInteger A = sqrtOfN + i;
                try
                {
                    x = CryptoUtils.Sqrt(BigInteger.Pow(A, 2) - N);
                }
                catch (ArithmeticException)
                {
                    continue;
                }

                BigInteger p = A - x;
                BigInteger q = A + x;
                BigInteger pq = p * q;
                if (pq == N)
                {
                    return p;
                }
            }
            throw new ArgumentException("Unable to calculate p and q");
        }

        //A = 3p+2q, factoring 24N
        private BigInteger Challenge3(BigInteger N)
        {
            BigInteger sqrtOf24TimesN = CryptoUtils.Sqrt(24*N);
            int scanRange = (int)Math.Pow(2, 20);
            for (var i = 0; i < scanRange; i++)
            {
                try
                {
                    BigInteger A = sqrtOf24TimesN + i;
                    BigInteger x = CryptoUtils.Sqrt(BigInteger.Pow(A, 2) - 24 * N);
                    BigInteger p = (A - x) / 6;
                    BigInteger q = (A - 3 * p) / 2;
                    BigInteger pq = p * q;
                    if (pq == N)
                    {
                        return p;
                    }
                }
                catch (ArithmeticException) { continue; }
                
            }
            throw new ArgumentException("Unable to find p and q");
        }

        private string DecryptRsa(BigInteger N)
        {
            BigInteger ciphertext = BigInteger.Parse("22096451867410381776306561134883418017410069787892831071731839143676135600120538004282329650473509424343946219751512256465839967942889460764542040581564748988013734864120452325229320176487916666402997509188729971690526083222067771600019329260870009579993724077458967773697817571267229951148662959627934791540");
            BigInteger e = BigInteger.Parse("65537");
            BigInteger A = CryptoUtils.Sqrt(N) + 1;
            BigInteger x = CryptoUtils.Sqrt(BigInteger.Pow(A, 2) - N);
            BigInteger p = A - x;
            BigInteger q = A + x;

            BigInteger fi = (p - 1) * (q - 1);

            BigInteger d = BigInteger.ModPow(e, fi - 2, fi); //d = e^-1 mod fi(n)
            BigInteger msg = BigInteger.ModPow(ciphertext, d, N);

            Byte[] pkcs15Message = BigInteger.ModPow(ciphertext, d, N).ToByteArray();

            string hex = CryptoUtils.ByteArrayToString(pkcs15Message.Reverse().ToArray());

            return hex;
        }
    }
}
