using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    class OrderInZ35 : IExecutable
    {
        public void Execute()
        {
            var order = 0;
            for (var i = 1; i < 35; i++)
            {
                if (Math.Pow(2, i) % 35 == 1)
                {
                    order = i;
                    break;
                }
            }
            Console.WriteLine(order);
        }
    }
}
