using System;
using System.Collections.Generic;
using System.Text;

namespace Crypt1.Algo_Parts
{
    public sealed class DES
    {
        private Mode crypt_mode;
        private byte[] crypt_key;
        private ISym_encrypt algorytm;

        public DES (Mode m, byte[] k, params object[] other)
        {
            crypt_mode = m;
            crypt_key = k;
            algorytm = new Feistel(new Round_Generation(), 
                                   new F_Function(),
                                   crypt_key);
        }

        public byte[] Encrypt(byte[] Indata)
        {
            byte[] eIndata = new byte[Indata.Length + 8 - (Indata.Length % 8)];
            byte[] Outdata = new byte[eIndata.Length];
            int lastind = 0;
            for (lastind = 0; lastind < Indata.Length; lastind++)
            {
                eIndata[lastind] = Indata[lastind];
            }
            int padding_num = lastind;
            for (; lastind < 8; lastind++)
            {
                eIndata[lastind] = (byte)(8 - padding_num);
            }
            //eIndata[lastind] = (byte)0x80;

            switch (crypt_mode)
            {
                case Mode.ECB:
                    {
                        byte[] block = new byte[8];
                        int posinblock = 0;
                        int posinOutData = 0;
                        for (int i = 0; i < eIndata.Length; i++)
                        {
                            block[posinblock] = eIndata[i];
                            posinblock++;
                            if (posinblock == 8)
                            {
                                posinblock = 0;
                                block = algorytm.encrypt(block);
                                foreach (byte b in block)
                                {
                                    Outdata[posinOutData] = b;
                                    posinOutData++;

                                }
                            }
                        }
                    }
                    break;
                case Mode.CBC:
                    break;
                case Mode.CFB:
                    break;
                case Mode.OFB:
                    break;
                case Mode.CTR:
                    break;
                case Mode.RD:
                    break;
                case Mode.RDH:
                    break;
                default:
                    break;
            }
            return Outdata;
        }

        public byte[] Decrypt(byte[] Indata)
        {
            byte[] Outdata = new byte[Indata.Length];
            switch (crypt_mode)
            {
                case Mode.ECB:
                    {
                        byte[] block = new byte[8];
                        int posinblock = 0;
                        int posinoutdata = 0;

                        for (int i = 0; i < Indata.Length; i++)
                        {
                            block[posinblock] = Indata[i];
                            posinblock++;
                            if (posinblock == 8)
                            {
                                posinblock = 0;
                                block = algorytm.decrypt(block);
                                foreach (byte b in block)
                                {
                                    Outdata[posinoutdata++] = b;
                                }

                            }

                        }
                    }
                    break;
                case Mode.CBC:
                    break;
                case Mode.CFB:
                    break;
                case Mode.OFB:
                    break;
                case Mode.CTR:
                    break;
                case Mode.RD:
                    break;
                case Mode.RDH:
                    break;
                default:
                    break;
            }


            return CutTail(Outdata);
        }

        public byte[] CutTail(byte[] data)
        {

            int i;
            for (i = data.Length - 1; i >= 0; i--)
            {
                if (data.Length - i == (byte)data[i])
                {
                    bool is_padding = true;
                    for (int j = i; j < data.Length - 1; j++)
                    {
                        if (data[j] != data[i])
                            is_padding = false;
                    }
                    if (!is_padding) i = 0;
                    break;
                }
            }
            byte[] outdata = new byte[data.Length - i];
            for (int j = 0; j < data.Length - i; j++)
                outdata[j] = data[j];
            return outdata;
        }

    }

    public enum Mode
    {
        ECB,
        CBC,
        CFB,
        OFB,
        CTR,
        RD,
        RDH
    };
}
