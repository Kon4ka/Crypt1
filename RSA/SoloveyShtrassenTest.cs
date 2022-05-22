using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Crypt1.Prime;

namespace Crypt1.SimpleTests
{
    public class SoloveyShtrassenTest : IPrimeTest
    {
        public bool SimplifyCheck(BigInteger number, double probability)
        {
            int T = 10;
            RandomNumberGenerator _random = RandomNumberGenerator.Create();

            if (number == 2 || number == 3)   //Проверка мелких чисел
                return true;
            if (number < 2 || number % 2 == 0)
                return false;

            for (int i = 0; i < T; i++)
            {
                BigInteger a = SimplePrime.GetRandomFromRange(_random, 2, number - 1);    //Выбираем большое случайное а по границам [2 до n-1]
                if (BigInteger.GreatestCommonDivisor(a, number) > 1) //Если НОД(a, n) > 1, тогда число составное
                    return false;
                BigInteger y = BigInteger.ModPow(a, (number - 1) / 2, number);    // Считаем х = а^((n-1)/2) mod n
                BigInteger x = LegandrYakobiService.GetYakobiSymbol(a, number);  // Получаем символ Якоби
                if (x < 0)      
                    x += number;
                if (y != x % number)     // Если у не остаток от деоения х на n то число составное.
                    return false;
            }

            return true;    // Если дошли, то число, вероятно простое
        }

        public int GetCountRounds(double probability)
        {
            throw new ArgumentException();
        }
    }
}
