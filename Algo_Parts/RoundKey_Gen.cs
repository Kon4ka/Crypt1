using System;
using System.Collections.Generic;
using System.Text;
using Crypt1;

namespace Crypt1.Algo_Parts
{
    class RoundKey_Gen: IRound_generation
    {
        public byte[][] Round_Keys(byte[] key)
        {
            int[] move = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            var result = new byte[16][];
            var expandedKey = ExpandStartKey(key);
            var permutedKey = Crypt_alg.P_Change(expandedKey, Matrix.p64_to_56);//C0
            var C = permutedKey;
            byte[] D = new byte[4];
            int j = 29;
            for (int i = 0; i < 28; i++)
            {
                Crypt_alg.SetFromPos(D, Crypt_alg.GetFromPos(permutedKey, j), i + 1);
                j++;
            }
            for (int i = 0; i < 16; i++)
            {
                C = Rotate(C, move[i]);
                D = Rotate(D, move[i]);
                var gluedkey = UnionCD(C, D);
                result[i] = Crypt_alg.P_Change(gluedkey, Matrix.KeySelect);
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

        private byte[] Rotate(byte[] partkey, int rotatecount)
        {
            var result = new byte[4];
            for (int i = 0; i < 28; i++)
            {
                Crypt_alg.SetFromPos(result, Crypt_alg.GetFromPos(partkey, (i + rotatecount) % 28 + 1), i + 1);
            }
            return result;
        }

        private byte[] UnionCD(byte[] part1, byte[] part2)
        {
            var result = new byte[7];
            for (int i = 0; i < 56; i++)
            {
                if (i >= 28)
                {
                    Crypt_alg.SetFromPos(result, Crypt_alg.GetFromPos(part2, i - 28 + 1), i + 1);
                }
                else
                {
                    Crypt_alg.SetFromPos(result, Crypt_alg.GetFromPos(part1, i + 1), i + 1);
                }
            }
            return result;

        }
    }
}
