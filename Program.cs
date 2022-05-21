using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Crypt1.Algo_Parts;

namespace Crypt1
{
    class Program
    {
        static void Main(string[] args)
        {
            Crypt_alg alg = new Crypt_alg();
            byte[] orig1 = { 2, 8, 6};
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
                
                //var res_S = Crypt_alg.S_Change(orig1, s_block1, k);
                var rest = Crypt_alg.P_Change(orig1, p_block);
/*                foreach (var @byte in rest)
                {
                    Console.WriteLine("result P: {0}", Convert.ToString(@byte, 2));
                }
                foreach (var @byte in res_S)
                {
                    Console.WriteLine("result S: {0}", Convert.ToString(@byte, 2));
                }*/
                //Round_Generation a = new Round_Generation();
                //var res = a.Round_Keys(ASCIIEncoding.ASCII.GetBytes("12345678"));



                byte[] kk = ASCIIEncoding.ASCII.GetBytes("12345678");
                DES d = new DES(Mode.ECB, kk);
                byte[] word = ASCIIEncoding.ASCII.GetBytes("Baalt");
                var shift = d.Encrypt(word);
                var res = d.Decrypt(shift);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
