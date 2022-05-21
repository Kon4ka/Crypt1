using System;
using System.Collections.Generic;
using System.Text;
using Crypt1;

namespace Crypt1
{
    class Round_Generation : IRound_generation
    {
        public byte[][] Round_Keys(byte[] orig_key)
        {
            byte[][] after_shift = new byte[16][];
            byte[][] result = new byte[16][];

            for (int i = 0; i < 16; i++)
            {
                result[i] = new byte[6];
                after_shift[i] = new byte[7];
            }

            //Array.Reverse(key, 0, key.Length);
            byte[] key = ExpandStartKey(orig_key);

            if (key.Length != 8)
            {
                throw new ArgumentException("Invalid key lenght");
            }
            int[] move = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            /*            int[,] p64_to_56 = {{ 57, 49, 41, 33, 25, 17, 9, 1 }, 
                                            { 58, 50, 42, 34, 26, 18, 10, 2},
                                            { 59, 51, 43, 35, 27, 19, 11, 3},
                                            { 60, 52, 44, 36, 63, 55, 47, 39},
                                            { 31, 23, 15, 7, 62, 54, 46, 38},
                                            { 30, 22, 14, 6, 61, 53, 45, 37},
                                            { 29, 21, 13, 5, 28, 20, 12, 4 }};
                        int[,] p56_to_48 = {{14, 17, 11, 24, 1, 5, 3, 28 },
                                            {15, 6, 21, 10, 23, 19, 12, 4 },
                                            {26, 8, 16, 7, 27, 20, 13, 2},
                                            {41, 52, 31, 37, 47, 55, 30, 40 },
                                            {51, 45, 33, 48, 44, 49, 39, 56 },
                                            {34, 53, 46, 42, 50, 36, 29, 32}};*/
            int[,] p64_to_56 = {{56,48,40,32,24,16,8,0},
                                {57,49,41,33,25,17,9,1},
                                {58,50,42,34,26,18,10,2},
                                {59,51,43,35,62,54,46,38},
                                {30,22,14,6,61,53,45,37},
                                {29,21,13,5,60,52,44,36},
                                {28,20,12,4,27,19,11,3}};
            int[,] p56_to_48 = {{13,16,10,23,0,4,2,27,},
                                {14,5,20,9,22,18,11,3},
                                {25,7,15,6,26,19,12,1},
                                {40,51,30,36,46,54,29,39},
                                {50,44,32,47,43,48,38,55},
                                {33,52,45,41,49,35,28,31,}};

            /*            for (int i = 0; i < p56_to_48.GetUpperBound(0)+1; i++)
                        {
                            Console.Write("{");
                            for (int j = 0; j < p56_to_48.GetUpperBound(1)+1; j++)
                            {
                                Console.Write(p56_to_48[i, j]-1 + ",");
                            }
                            Console.Write("},\n");
                        }*/
            byte[] key_56 = Crypt_alg.P_Change(key, p64_to_56);
            //ulong ukey = BitConverter.ToUInt64(key_56);
            byte[] D = { 0, 0, 0, 0 };
            byte[] C = { 0, 0, 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                C[i] = key_56[i];
                D[i + 1] = key_56[i + 4];
            }
            C[3] = (byte)(key_56[3] & 240);
            D[0] = (byte)(key_56[3] & 15);

            for (int i = 0; i < 4; i++)
            {
                Crypt_alg.RotateRight(C, false);
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < move[i]; j++)
                {
                    Crypt_alg.RotateLeft(C, false);
                    Crypt_alg.RotateLeft(D, false);
                }
                for (int j = 0; j < 4; j++)
                {
                    Crypt_alg.RotateLeft(C, true);
                }
                for (int j = 0; j < 3; j++)
                {
                    after_shift[i][j] = C[j];
                    after_shift[i][j + 4] = D[j + 1];
                }
                after_shift[i][3] = (byte)(C[3] | D[0]);
            }

            for (int i = 0; i < 16; i++)
            {
                result[i] = Crypt_alg.P_Change(after_shift[i], p56_to_48);
            }

            return result;
        }

        private byte[] ExpandStartKey(byte[] key)
        {
            byte[] result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                int countone = 0;
                for (int j = 0; j < 7; j++)
                {
                    var bit = Crypt_alg.GetFromPos(key, j + i * 7 + 1);
                    if (bit)
                        countone++;
                    Crypt_alg.SetFromPos(result, bit, j + i * 8 + 1);
                }
                if (countone % 2 == 1)
                {
                    Crypt_alg.SetFromPos(result, false, 8 * (i + 1));
                }

                else
                {
                    Crypt_alg.SetFromPos(result, true, 8 * (i + 1));
                }
            }
            return result;
        }
    }
}
