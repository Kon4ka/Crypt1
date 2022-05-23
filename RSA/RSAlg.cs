using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Crypt1.Prime;
using Crypt1.Algo_Parts;

namespace Crypt1.RSA
{
    public class RSAlg : CryptoAlgBase
    {
        private BigInteger _n;
        private BigInteger _d;
        private SimplePrime _keyGenerate;

        public RSAlg (double probability, int lenght, PrimeTestMode mode)
        {
            _keyGenerate = new SimplePrime(mode, probability, lenght);
        }

        public string Encrypt(string data)
        {
            BigInteger q = _keyGenerate.GeneratePrimeDigit();
            BigInteger p = _keyGenerate.GeneratePrimeDigit();

            BigInteger n = p * q;
            BigInteger m = (p - 1) * (q - 1);
            BigInteger d = m - 1;   //Winner protect
            BigInteger e = 10;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) 
                {
                    d--;
                    i = 1;
                }

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }
            List<string> result = new List<string>();

            _d = d;
            _n = n;

            BigInteger bi;

            for (int i = 0; i < data.Length; i++)
            {
                int index = (char)data[i];

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }
            StringBuilder s = new StringBuilder();
            foreach (var item in result)
            {
                s.Append(item);
                s.Append("\n");
            }
            return s.ToString();
        }

        public string Decrypt(string data)
        {
            BigInteger d = _d;
            BigInteger n = _n;
            string result = "";

            List<string> input = new List<string> (data.Split("\n"));
            input.Remove("");
            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += (char)index;
            }
            return result;
        }

    }
}
