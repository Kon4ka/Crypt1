using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypt1.SimpleTests
{
    public interface IPrimeTest
    {
        bool SimplifyCheck(BigInteger number, double probability);

        int GetCountRounds(double probability);


    }
}
