using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Crypt1.Algo_Parts;
using System.IO;
using System.Threading.Tasks;
using System.Numerics;

namespace Crypt1
{
    class Program
    {
        static async Task Main(string[] args)
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
                
                var rest = Crypt_alg.P_Change(orig1, p_block);


                byte[] kk = ASCIIEncoding.ASCII.GetBytes("12345678");
                DES d = new DES(Mode.ECB, kk);
                DES dcbc = new DES(Mode.CFB, kk);
                byte[] word = ASCIIEncoding.ASCII.GetBytes("Baalt");

                dcbc.SetIV();
                var en = dcbc.Encrypt(word);
                var de = dcbc.Decrypt(en);

                string wordstr = ASCIIEncoding.ASCII.GetString(de);

                string path = Directory.GetCurrentDirectory()+ "/2bildscet2.png";
                string path1 = Directory.GetCurrentDirectory() + "/2bildscet2Crypt.png";
                byte[] buffer;

                using (FileStream fstream = File.OpenRead(path))
                {
                    buffer = new byte[fstream.Length];
                    await fstream.ReadAsync(buffer, 0, buffer.Length);

                }
                // Кодировка изображения, раскоментить при необходимости
                /*var shift = d.Encrypt(buffer);
                var res = d.Decrypt(shift);

                using (FileStream fstream = new FileStream(path1, FileMode.OpenOrCreate))
                {

                    await fstream.WriteAsync(res, 0, res.Length);
                    Console.WriteLine("Текст записан в файл");
                }*/



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
