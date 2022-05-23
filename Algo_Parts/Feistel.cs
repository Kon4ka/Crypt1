using System;
using System.Collections.Generic;
using System.Text;


namespace Crypt1.Algo_Parts
{
    public class Feistel : ISym_Encrypt
    {
        private IRound_generation round_keys_gen;
        private IF_function Feistel_f;
        private byte[][] round_keys;

        public Feistel (IRound_generation key, IF_function f, byte[] orig_key)
        {
            round_keys_gen = key;
            Feistel_f = f;
            round_keys = key.Round_Keys(orig_key);
        }
        public byte[] Decrypt(byte[] block)
        {
            int[] round_order = new int[16];
            for (int i = 0; i < 16; i++)
            {
                round_order[i] = 15 - i;
            }
            return crypt(block, round_order);
        }

        public byte[] Encrypt(byte[] block)
        {
            int[] round_order = new int[16];
            for (int i = 0; i < 16; i++)
            {
                round_order[i] = i;
            }
            return crypt(block, round_order);
        }

        private byte[] crypt(byte[] block, int[] round_order)
        {
            block = Crypt_alg.P_Change(block, Matrix.IP);
            var R = new byte[block.Length / 2];

            //R and L separate
            for (int i = 4; i < 8; i++)
            {
                R[i - 4] = block[i];
            }
            var L = block;

            //Rounds
            for (int i = 0; i < 16; i++)
            {
                byte[] tmp;
                if (round_order[0] == 0)
                {
                    tmp = Crypt_alg.XOR(Feistel_f.transform(R, round_keys[round_order[i]]), L);
                    L = R;
                    R = tmp;
                }
                else
                {
                    tmp = Crypt_alg.XOR(Feistel_f.transform(L, round_keys[round_order[i]]), R);
                    R = L;
                    L = tmp;
                }
            }
            byte[] res = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                if (i >= 4)
                {
                    res[i] = R[i - 4];
                }
                else
                {
                    res[i] = L[i];
                }
            }
            res = Crypt_alg.P_Change(res, Matrix.IPReverse);
            return res;
        }
    }
}
