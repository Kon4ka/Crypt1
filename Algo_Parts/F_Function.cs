using System;
using System.Collections.Generic;
using System.Text;
using Crypt1;

namespace Crypt1.Algo_Parts
{
    class F_Function : IF_function
    {
        public byte[] transform(byte[] block, byte[] round_key)
        {
            var permutedBlock = Crypt_alg.P_Change(block, Matrix.E);

            var xoredBlock = Crypt_alg.XOR(permutedBlock, round_key);

            var stransformedBlock = Crypt_alg.S_Change(xoredBlock);

            return Crypt_alg.P_Change(stransformedBlock, Matrix.P);
        }
    }
}
