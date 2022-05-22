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
    class MillerRabinTest: IPrimeTest
    {
        public bool SimplifyCheck(BigInteger number, double probability)
        {
            int T = 10;
            RandomNumberGenerator _random = RandomNumberGenerator.Create();
            int s = 0;
            BigInteger t = number - 1;

            if (number == 2 || number == 3)       //Проверка ранних чисел
                return true;
            if (number < 2 || number % 2 == 0)
                return false;


            while (t % 2 == 0)  //Представис n − 1 в виде (2^s)·t, где t нечётно последовательным делением n - 1 на 2
            {
                t /= 2;
                s += 1;
            }
            for (int i = 0; i < T; i++)
            {
                BigInteger a = SimplePrime.GetRandomFromRange(_random, 2, number - 2);    // Выбираем случайное число в диапазоне
                BigInteger x = BigInteger.ModPow(a, t, number);                  // Вычисляем x = a^t mod n
                if (x == 1 || x == number - 1)  //Пропускаем итерацию если x == 1 || x == number - 1
                    continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);    //Пересчитываем х = x^2 mod n
                    if (x == 1)
                        return false;       //Число составное если х = 1
                    if (x == number - 1) //Выходим из цикла
                        break;
                }
                if (x != number - 1) //Число составное если х != number - 1
                    return false;
            }
            return true;        // Если дошли то число вероятно простое
        }

        public int GetCountRounds(double probability)
        {
            throw new ArgumentException();
        }
        
    }
}
