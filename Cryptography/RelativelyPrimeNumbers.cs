using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class RelativelyPrimeNumbers : IExecutable
    {
        //bruteforce
        public void Execute()
        {
            for (var a = 1; a < 50; a++)
            {
                for (var b = -50; b < 50; b++)
                {
                    int result = 7 * a + 23 * b;
                    if (result == 1)
                    {
                        Console.WriteLine("a: {0}, b: {1}", a, b);
                        return;
                    }
                }
            }
            Console.WriteLine("Unsuccesfull");
        }
    }
}
