using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crypt1
{
    sealed class Crypt_alg
    {
        public Crypt_alg() { }
        public byte[] P_Change(byte[] orig, int[,] p_block)
        {
            if (p_block is null || p_block.GetUpperBound(1) + 1 != 8) 
            {
                throw new ArgumentException("P Block is not for bytes or null");
            }

            int size = p_block.GetUpperBound(0) + 1;

            byte[] res = new byte[size];

            //Console.WriteLine(Convert.ToString(orig[0], 2));

            for (int i = 0; i < size; i++)
            {
                res[i] = 0;
                for (int j = 0; j < p_block.GetUpperBound(1) + 1; j++)
                {
                    uint bit_to_end = 0;
                    byte bit = 0;

                    int id_byte = p_block[i, j] / (p_block.GetUpperBound(1) + 1);
                    int id_in_byte = p_block[i, j] % (p_block.GetUpperBound(1) + 1);

                    bit_to_end = (byte)(orig[id_byte] >> (p_block.GetUpperBound(1) - id_in_byte));
                    bit = (byte)(bit_to_end & 1);
                    res[i] = (byte)(res[i] | (bit << (p_block.GetUpperBound(1) - j)));
                    //Console.WriteLine("res[i] {0}", Convert.ToString(res[i], 2));
                }
            }

            return res;
        }

        public byte[] S_Change(byte[] some_orig, Dictionary<byte, byte> s_block, int k)
        {
            byte[] orig = new byte[some_orig.Length];
            Array.Copy(some_orig, orig, some_orig.Length);

            int result = s_block.Values.Max();

            if (s_block is null || orig is null || result > (1 << k))
            {
                throw new ArgumentException("S Block is not for bytes or null");
            }

            //Узнать какой байт на какой меняем

            for (int i = 0; i < orig.Length*8; i += k)
            {
                byte cur_k_part = 0;
                for (int j = 0; j < k; j++)
                {
                    uint bit_to_end = 0;
                    byte bit = 0;

                    int id_byte = i / 8;
                    int id_in_byte = i  % 8 + j;

                    bit_to_end = (byte)(orig[id_byte] >> (8 - 1 - id_in_byte));
                    bit = (byte)(bit_to_end & 1);
                    cur_k_part = (byte)(cur_k_part << 1);
                    cur_k_part = (byte)(cur_k_part | bit);
                }

                if ((i/8) != ((i+k-1)/8))//!
                {
                    int to_end = 8 - i % 8;
                    if (s_block.ContainsKey(cur_k_part))
                    {
                        byte b1 = (byte)(s_block[cur_k_part] >> k - to_end);
                        byte b2 = (byte)(s_block[cur_k_part] << 8 - (k - to_end));
                        int len1 =  to_end;     //----
                        int len2 = k - len1;

                        orig[i / 8] = (byte)(orig[i / 8] & (255 << len1));
                        orig[(i + k) / 8] = (byte)(orig[(i + k) / 8] & (255 >> len2));
                        orig[i / 8] = (byte)(orig[i / 8] | b1);
                        orig[(i + k) / 8] = (byte)(orig[(i + k) / 8] | b2);

                    }
                }
                else
                {
                    int to_start = (8 - k) - i % 8;

                    if (s_block.ContainsKey(cur_k_part))
                    {
                        byte mask = 0;
                        byte new_b = (byte)(s_block[cur_k_part] << to_start);
                        for (int j = 0; j < k; j++)
                        {
                            mask = (byte)(mask | 1);
                            mask = (byte)(mask << 1);
                        }
                        mask = (byte)(mask >> 1);
                        mask = (byte)(mask << (8 - i%8 - k));
                        orig[i / 8] = (byte)(orig[i / 8] & ~mask);
                        orig[i / 8] = (byte)(orig[i / 8] | new_b);
                    }

                }
            }

            return orig;
        }


        public byte[,] Round_Keys(byte[] key)
        {
            byte[,] result;
            Array.Reverse(key, 0, key.Length);
            if (key.Length != 8)
            {
                throw new ArgumentException("Invalid key lenght");
            }
            int[] move = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            int[,] p64_to_56 = {{ 57, 49, 41, 33, 25, 17, 9, 1 }, 
                                { 58, 50, 42, 34, 26, 18, 10, 2},
                                { 59, 51, 43, 35, 27, 19, 11, 3},
                                { 60, 52, 44, 36, 63, 55, 47, 39},
                                { 31, 23, 15, 7, 62, 54, 46, 38},
                                { 30, 22, 14, 6, 61, 53, 45, 37},
                                { 29, 21, 13, 5, 28, 20, 12, 4 }};
            byte[] key_56 = P_Change(key, p64_to_56);
            //ulong ukey = BitConverter.ToUInt64(key_56);
            byte[] C, D;
            //Array.Copy(key_56, 0, C, 0, 4)
            for (int i = 0; i < 16; i++)
            {
                RotateRight(key);
            }

            return null;
        }

        /// <summary>
        /// Rotates the bits in an array of bytes to the left.
        /// </summary>
        /// <param name="bytes">The byte array to rotate.</param>
        public static void RotateLeft(byte[] bytes)
        {
            bool carryFlag = ShiftLeft(bytes);

            if (carryFlag == true)
            {
                bytes[bytes.Length - 1] = (byte)(bytes[bytes.Length - 1] | 0x01);
            }
        }

        /// <summary>
        /// Rotates the bits in an array of bytes to the right.
        /// </summary>
        /// <param name="bytes">The byte array to rotate.</param>
        public static void RotateRight(byte[] bytes)
        {
            bool carryFlag = ShiftRight(bytes);

            if (carryFlag == true)
            {
                bytes[0] = (byte)(bytes[0] | 0x80);
            }
        }

        /// <summary>
        /// Shifts the bits in an array of bytes to the left.
        /// </summary>
        /// <param name="bytes">The byte array to shift.</param>
        public static bool ShiftLeft(byte[] bytes)
        {
            bool leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for (int index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = (bytes[index] & 0x80) > 0;

                if (index > 0)
                {
                    if (carryFlag == true)
                    {
                        // Apply the carry to the rightmost bit of the current bytes neighbor to the left.
                        bytes[index - 1] = (byte)(bytes[index - 1] | 0x01);
                    }
                }
                else
                {
                    leftMostCarryFlag = carryFlag;
                }

                bytes[index] = (byte)(bytes[index] << 1);
            }

            return leftMostCarryFlag;
        }

        /// <summary>
        /// Shifts the bits in an array of bytes to the right.
        /// </summary>
        /// <param name="bytes">The byte array to shift.</param>
        public static bool ShiftRight(byte[] bytes)
        {
            bool rightMostCarryFlag = false;
            int rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for (int index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = (bytes[index] & 0x01) > 0;

                if (index < rightEnd)
                {
                    if (carryFlag == true)
                    {
                        // Apply the carry to the leftmost bit of the current bytes neighbor to the right.
                        bytes[index + 1] = (byte)(bytes[index + 1] | 0x80);
                    }
                }
                else
                {
                    rightMostCarryFlag = carryFlag;
                }

                bytes[index] = (byte)(bytes[index] >> 1);
            }

            return rightMostCarryFlag;
        }
    }
}
