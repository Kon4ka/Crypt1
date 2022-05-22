using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Crypt1.SimpleTests
{
	public static class LegandrYakobiService
	{

		public static BigInteger GetLegandrSymbol(BigInteger a, BigInteger p)
		{
			if (p <= 2)
				throw new Exception("Р <= 2");
			if (a == 0 || a == 1)
				return a;

			BigInteger r;
			if (a % 2 == 0)
			{
				r = GetLegandrSymbol(a / 2, p);
				if (((p * p - 1) & 8) != 0)
					r *= -1;
			}
			else
			{
				r = GetLegandrSymbol(p % a, a);
				if (((a - 1) * (p - 1) & 4) != 0)
					r *= -1;
			}
			return r;
		}


		public static BigInteger GetYakobiSymbol(BigInteger a, BigInteger b)
		{

			if (BigInteger.GreatestCommonDivisor(a, b) != 1)
				return 0;
			int r = 1;
			if (a < 0)
			{
				a = -a;
				if (b % 4 == 3)
					r = -r;
			}

			do
			{
				int t = 0;
				while (a % 2 == 0)
				{
					t++;
					a = a / 2;
				}
				if (t % 2 == 1)
				{
					var mod = b % 8;
					if (mod == 3 || mod == 5)
						r = -r;
				}

				var amod4 = a % 4;

				if (amod4 == b % 4 && amod4 == 3)
				{
					r = -r;
				}

				var c = a;
				a = b % c;
				b = c;
			} while (a != 0);

			return r;
		}


	}
}
