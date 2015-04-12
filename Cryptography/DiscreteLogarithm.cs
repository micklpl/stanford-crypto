using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Cryptography
{
    public class DiscreteLogarithm : IExecutable
    {
        public void Execute()
        {
            BigInteger p = BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084171");
            BigInteger g = BigInteger.Parse("11717829880366207009516117596335367088558084999998952205599979459063929499736583746670572176471460312928594829675428279466566527115212748467589894601965568");
            BigInteger h = BigInteger.Parse("3239475104050450443565264378728065788649097520952449527834792452971981976143292558073856937958553180532878928001494706097394108577585732452307673444020333");

            BigInteger x = ComputeX(g, h, p);
            Console.WriteLine(x.ToString());
        }

        private BigInteger ComputeX(BigInteger g, BigInteger h, BigInteger p)
        {
            int maxx1 = 1048576; //2^20
            long B = maxx1;
            BigInteger[] lookupValues = new BigInteger[maxx1 + 1];
            var now = DateTime.Now;

            var results = from x1 in Enumerable.Range(0, maxx1 + 1).AsParallel()
                          select new
                          {
                              x1,
                              value = CalculateLeftSideValue(x1, p, h, g)
                          };

            foreach (var result in results)
            {
                lookupValues[result.x1] = result.value;
            }

            TimeSpan ts = DateTime.Now - now;

            Console.WriteLine("Lookup built succesfully in " + ts.ToString());

            for (var x0 = 0; x0 <= maxx1; x0++)
            {
                long Bx0 = B * x0;
                BigInteger value = BigInteger.ModPow(g, Bx0, p);
                for (var x1 = 0; x1 < lookupValues.Length; x1++)
                {
                    if (lookupValues[x1] == value)
                    {
                        return new BigInteger(Bx0 + x1);
                    }
                }
            }

            throw new NotImplementedException("Error in implementation");
        }

        private BigInteger CalculateLeftSideValue(int x1, BigInteger p, BigInteger h, BigInteger g)
        {
            var power = BigInteger.ModPow(g, x1, p);
            var inverted = BigInteger.ModPow(power, p - 2, p);                
            return BigInteger.Multiply(h, inverted) % p;
        }
    }
}
