using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Cipher
{

    /// <summary>
    /// @LJ Gomez 20170925
    /// </summary>
    public class ShiftCipher
    {
        private string CharString;

        public enum Mode
        {
            Encrypt,
            Decrypt
        }

        public ShiftCipher()
        {
            var gg = new List<char>();
            for (var i = 1; i <= 0x3ff; i++)
            {
                gg?.Add(Convert.ToChar(i));
            }

            CharString = gg.Aggregate(new StringBuilder(), (sb, a) => sb.Append(a)).ToString();
        }

        public string Cipher(string val, string key, Mode CipherMode = Mode.Encrypt)
        {
            //var jh = new List<char>();
            char[] chr = new char[val.Length];

            for (var i = 0; i < val.Length; i++)
            {
                //var x = GetCharIndex(val[i]);
                //var k = GetStringIndex(key);
                //var s = CharString.Length; //Salt;
                //int y = 0;
                //switch (CipherMode)
                //{
                //    case Mode.Encrypt:
                //        y = (x + k) ^ s;
                //        break;
                //    case Mode.Decrypt:
                //        y = Math.Abs(x - k);
                //        //y = y == 0 ? 0 : (s - y); // ^ s;
                //        break;
                //}

                //jh.Add(GetCharFromIndex(y));
                if (CipherMode == Mode.Encrypt)
                {
                    int j = val[i] - CharString.Length;
                    chr[i] = CharString[j];
                } else
                {
                    int j = CharString.IndexOf(val[i]) + CharString.Length;
                    chr[i] = (char)j;
                }
            }

            return new string(chr);
        }

        private char GetCharFromIndex(int y)
        {
            return CharString[y];
        }

        private int GetStringIndex(string key)
        {
            var h = 0;
            for (var i = 0; i < key.Length; i++)
            {
                h += GetCharIndex(key[i]);
            }
            return h;
        }

        private int GetCharIndex(char v)
        {
            for (var i = 0; i < CharString.Length; i++)
            {
                if (CharString[i] == v)
                    return i;
            }
            return -1;
        }

        public static string Encrypt(string val, string key) => new ShiftCipher().Cipher(val, key);

        public static string Decrypt(string val, string key) => new ShiftCipher().Cipher(val, key, Mode.Decrypt);

    }
}
