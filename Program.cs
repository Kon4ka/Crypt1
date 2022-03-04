using System;
using System.Collections.Generic;

namespace Crypt1
{
    class Program
    {
        static void Main(string[] args)
        {
            Crypt_alg alg = new Crypt_alg();
            byte[] orig1 = { 2, 8, 6};
            int[,] p_block = { { 6, 1, 2, 3, 4, 5, 12, 6 }, { 0, 1, 2, 3, 4, 5, 6, 7 } };

            var s_block1 = new Dictionary<byte, byte>()
            {
                { 0, 7},
                { 2, 1},
                { 4, 5}
            };

            byte[] orig2 = { 255, 200, 135 };
            var s_block2 = new Dictionary<byte, byte>()
            {
                { 15, 9},
                { 8, 15},
            };

            int k = 4;

            try
            {
                var res_S = alg.S_Change(orig2, s_block2, k);
                var res = alg.P_Change(orig1, p_block);
                foreach (var @byte in res)
                {
                    Console.WriteLine("result P: {0}", Convert.ToString(@byte, 2));
                }
                foreach (var @byte in res_S)
                {
                    Console.WriteLine("result S: {0}", Convert.ToString(@byte, 2));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
