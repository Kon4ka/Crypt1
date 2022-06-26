using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Crypt1.Algo_Parts;
using System.IO;
using System.Threading.Tasks;
using System.Numerics;
using Crypt1.RSA;
using Crypt1.Prime;
using Crypt1.SimpleTests;

namespace Crypt1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Crypt_alg alg = new Crypt_alg();
            byte[] orig1 = { 2, 8, 6 };
            int[,] p_block = { { 6, 1, 2, 3, 4, 5, 12, 6 }, { 0, 1, 2, 3, 4, 5, 6, 7 } }; //Todo +1 all

            var s_block1 = new Dictionary<byte, byte>()
            {
                { 0, 7},
                { 2, 1},
                { 4, 5}
            };

            byte[] orig2 = { 255, 200, 135, 15, 255, 200, 135, 15 };
            var s_block2 = new Dictionary<byte, byte>()
            {
                { 15, 9},
                { 8, 15},
            };

            ulong key = 2048 + 71;
            int k = 4;

            try
            {

                /*                var rest = Crypt_alg.P_Change(orig1, p_block);


                                byte[] kk = ASCIIEncoding.ASCII.GetBytes("12345678");
                                DES d = new DES(Mode.ECB, kk);
                                DES dcbc = new DES(Mode.CFB, kk);
                                RSAlg rsa = new RSAlg(0.98, 15, PrimeTestMode.Ferm);
                                byte[] word = ASCIIEncoding.ASCII.GetBytes("Baalt");

                                dcbc.SetIV();
                                var en = dcbc.Encrypt(word);
                                var de = dcbc.Decrypt(en);

                                var cryptstr = rsa.Encrypt("АРБУЗ");
                                var decryptstr = rsa.Decrypt(cryptstr);

                                string wordstr = ASCIIEncoding.ASCII.GetString(de);

                                string path = Directory.GetCurrentDirectory()+ "/2bildscet2.png";
                                string path1 = Directory.GetCurrentDirectory() + "/2bildscet2Crypt.png";
                                byte[] buffer;

                                using (FileStream fstream = File.OpenRead(path))
                                {
                                    buffer = new byte[fstream.Length];
                                    await fstream.ReadAsync(buffer, 0, buffer.Length);

                                }

                                var Dres = WienersАttack.WienerАttack(75668410412, 9626055260015);
                                if (Dres != BigInteger.Zero)
                                {
                                    Console.WriteLine($"D is {Dres}");
                                }*/
                // Кодировка изображения, раскоментить при необходимости
                /*var shift = d.Encrypt(buffer);
                var res = d.Decrypt(shift);

                using (FileStream fstream = new FileStream(path1, FileMode.OpenOrCreate))
                {

                    await fstream.WriteAsync(res, 0, res.Length);
                    Console.WriteLine("Текст записан в файл");
                }*/


                var t = quickModPow(288, 65, 131);
                var f = ch(new int[] { 1, 2, 5 }, new int[] { 7, 5, 6 });

                int x = 0;
                int y = 0;
                var dsf = gcd(4642855, 9992628, out x, out y);
                var sdfgs = GFG.gcdExtended(4642855, 9992628, out x, out y);

                var an = LegandrYakobiService.GetLegandrSymbol(new BigInteger(288), new BigInteger(131));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static int quickModPow(int a, int e, int m)
        {
            int r = 1;
            while (e != 0)
            {
                if ((e & (int)1) == 1)
                    r = (r * a) % m;
                e = e >> 1;
                a = (a * a) % m;
            }
            return r;
        }

        public static int ch(int[] r, int[] a)
        {
            int x = 0;
            int prod = 1;
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine($"For {a[i]}");
                for (int k = 0; k < a[i]; k++)
                {//Console.WriteLine((x + prod * k) % a[i]);
                    Console.WriteLine($"R - {r[i]} ___ r%a = {r[i] % a[i]}");
                    if (((x + prod * k) % a[i]) == (r[i] % a[i]))
                    {
                        x += prod * k;
                        prod *= a[i];
                        break;
                    }
                }
                Console.WriteLine("_____");
            }
            return x;
        }
        static int gcd(int a, int b, out int x, out int y)
        {
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }
            int x1, y1;
            int d = gcd(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }


    }

    class GFG
    {

        // extended Euclidean Algorithm
        public static int gcdExtended(int a, int b,
                                      out int x, out int y)
        {
            // Base Case
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            // To store results of
            // recursive call
            int x1 = 1, y1 = 1;
            int gcd = gcdExtended(b % a, a, out x1, out y1);

            // Update x and y using
            // results of recursive call
            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }
    }
}
