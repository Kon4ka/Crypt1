using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Crypt1.RSA
{
    public static class WienersАttack
    {

        //Fraction - list (up, bown)

        public static BigInteger WienerАttack(BigInteger curN, BigInteger curE)
        {
            // Вехний предел для D (по теореме Винера)
            var limitD = Sqrt(curN);
            limitD = Sqrt(limitD) / 3;
            var myM = 0x01010101;


            // Непрерывная дробь (разложили Е/N)
            IEnumerable<BigInteger> contFr = ContinuedFraction(curE, curN);
            // Подходящая дробь (знаменатель)
            IEnumerable<List <BigInteger>> convf = ConvergingFractions(contFr);
            List<List<BigInteger>> convfList = convf.ToList();

            Console.WriteLine("Верхний предел D - " + limitD);

            // Проверяем все подходящие дроби
            for (var i = 0; i < convfList.Count; i++)
            {
                // Текущая дробь для проверки
                var curFraction = convfList[i];

                if (curFraction[0] > limitD) 
                    break;

                var isC = BigInteger.ModPow(myM, curE, curN); // С = М^E mod N
                var myM2 = BigInteger.ModPow(isC, curFraction[1], curN); // M = C^D mod N

                // Нашли D в одном из знаменателей.
                if (myM == myM2)
                {
                    return curFraction[1];
                }
            }
            return BigInteger.Zero; 
        }

        private static IEnumerable<List<BigInteger>> ConvergingFractions(IEnumerable<BigInteger> curfrac)
        {
            BigInteger r = 0;
            BigInteger up = 1;
            BigInteger down = 0;
            BigInteger s = 1;
            foreach (var curr in curfrac)
            {
                s = down;
                r = up;

                // Рекурентное соотношение для вычисления подходящих дробей
                down = curr * down + s;
                up = curr * up + r;
                yield return new List<BigInteger>() { up, down };
            }
        }

        private static IEnumerable<BigInteger> ContinuedFraction(BigInteger up, BigInteger down)
        {
            while (down != 0)
            {
                var n = up / down;
                yield return n;
                down = up - down * n;
                up = down;
            }
        }

        public static BigInteger Sqrt(this BigInteger number)
        {
            if (number == 0) 
                return 0;
            if (number > 0)
            {
                //Верх. Гран. Log(num,2)
                double upb = Math.Ceiling(BigInteger.Log(number, 2));
                int bitLength = Convert.ToInt32(upb);
                BigInteger root = BigInteger.One << (bitLength / 2); //new BigInteger(1)

                while (!InSqrtRange(number, root))
                {
                    root += number / root;
                    root /= 2;
                }
                return root;
            }

            return BigInteger.Zero;
        }
        private static Boolean InSqrtRange(BigInteger number, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (number < upperBound && number >= lowerBound);
        }
    }
}
