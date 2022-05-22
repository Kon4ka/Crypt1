using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crypt1.Algo_Parts;

namespace Crypt1
{
    sealed class Crypt_alg
    {
        public Crypt_alg() { }

        //LAB1
        public static byte[] P_Change(byte[] orig, int[,] p_block)
        {
            if (p_block is null || p_block.GetUpperBound(1) + 1 != 8) 
            {
                throw new ArgumentException("P Block is not for bytes or null");
            }

            int size = p_block.GetUpperBound(0) + 1;

            byte[] res = new byte[size];

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

        public static byte[] S_Change(byte[] orig)
        {
            byte[] res = new byte[] { 0, 0, 0, 0 };

            for (int i = 0; i < 8; i++)
            {
                var bit1 = GetFromPos(orig, i + 1) ? 2 : 0;
                var bit6 = GetFromPos(orig, i + 6) ? 1 : 0;
                var row = bit1 | bit6;

                var bit2 = GetFromPos(orig, i + 2) ? 8 : 0;
                var bit3 = GetFromPos(orig, i + 3) ? 4 : 0;
                var bit4 = GetFromPos(orig, i + 4) ? 2 : 0;
                var bit5 = GetFromPos(orig, i + 5) ? 1 : 0;
                var col = bit2 | bit3 | bit4 | bit5;

                int number = Matrix.S[i, 16 * row + col];
                if (i % 2 == 0)
                {
                    res[i / 2] |= (byte)(number << 4);
                }
                else
                {
                    res[i / 2] |= (byte)(number);
                }
            }
            return res;
        }

        public static void SetFromPos(byte[] block, bool number, int pos)
        {
            if (number)
                block[(pos - 1) / 8] |= (byte)(1 << (7 - (pos - 1) % 8));
            else
                block[(pos - 1) / 8] &= (byte)(~(1 << (7 - (pos - 1) % 8)));
        }

        public static bool GetFromPos(byte[] block, int pos)
        {
            return (block[(pos - 1) / 8] & (1 << (7 - (pos - 1) % 8))) != 0;
        }

        public static void RotateLeft(byte[] bytes, bool is_rotatable_first_4)
        {
            bool carryFlag = ShiftLeft(bytes, is_rotatable_first_4);

            if (carryFlag == true)
            {
                bytes[bytes.Length - 1] = (byte)(bytes[bytes.Length - 1] | 0x01);
            }
        }

        public static void RotateRight(byte[] bytes, bool is_rotatable_first_4)
        {
            bool carryFlag = ShiftRight(bytes);

            if (carryFlag == true)
            {
                if(is_rotatable_first_4)
                    bytes[0] = (byte)(bytes[0] | 0x80);
                else
                    bytes[0] = (byte)(bytes[0] | 0x08);
            }
        }

        public static bool ShiftLeft(byte[] bytes, bool is_rotatable_first_4)
        {
            bool leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for (int index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                bool carryFlag;
                if (!is_rotatable_first_4)
                    if (index == 0)
                        carryFlag = (bytes[index] & 0x08) > 0;
                    else
                        carryFlag = (bytes[index] & 0x80) > 0;
                else 
                    carryFlag = (bytes[index] & 0x80) > 0;

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

                bytes[index] = (byte)((bytes[index] << 1));
                if (index == 0)
                        bytes[index] = (byte)(bytes[index] & (15));
            }

            return leftMostCarryFlag;
        }

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

        public static byte[] XOR(byte[] part1, byte[] part2)
        {
            var size = Math.Min(part1.Length, part2.Length);
            byte[] result = new byte[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = (byte)(part1[i] ^ part2[i]);
            }
            return result;
        }
    }


    //LAB1
    public interface IRound_generation
    {
        public byte[][] Round_Keys(byte[] key);
    }

    public interface IF_function
    {
        byte[] transform(byte[] block, byte[] round_key);
    }

    public interface ISym_encrypt
    {
        byte[] encrypt(byte[] block);   // без _. С заглав буквы
        byte[] decrypt(byte[] block);
    }
}
