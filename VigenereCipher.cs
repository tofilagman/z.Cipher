using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace z.Cipher
{
    /// <summary>
    /// LJ20170919
    /// </summary>
    public class VigenereCipher
    {

        private readonly string CharString;

        public List<CipherCtx> CipherDictionary;

        private static readonly int[,] NumDic = new int[10, 10] {
        /*0     0  1  2  3  4  5  6  7  8  9 */
        /*0*/  {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        /*1*/  {1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
        /*2*/  {2, 3, 4, 5, 6, 7, 8, 9, 0, 1},
        /*3*/  {3, 4, 5, 6, 7, 8, 9, 0, 1, 2},
        /*4*/  {4, 5, 6, 7, 8, 9, 0, 1, 2, 3},
        /*5*/  {5, 6, 7, 8, 9, 0, 1, 2, 3, 4},
        /*6*/  {6, 7, 8, 9, 0, 1, 2, 3, 4, 5},
        /*7*/  {7, 8, 9, 0, 1, 2, 3, 4, 5, 6},
        /*8*/  {8, 9, 0, 1, 2, 3, 4, 5, 6, 7},
        /*9*/  {9, 0, 1, 2, 3, 4, 5, 6, 7, 8}
            };

        public VigenereCipher()
        {
            var gg = new List<char>();
            for (var i = 1; i <= 0x3ff; i++)
            {
                gg?.Add(Convert.ToChar(i));
            }

            CharString = gg.Aggregate(new StringBuilder(), (sb, a) => sb.Append(a)).ToString();
        }

        public enum Mode
        {
            Encrypt,
            Decrypt
        }

        public string Cipher(string StrVal, string Key, Mode CipherMode = Mode.Encrypt)
        {
            string ky = ReGenerateKey(Key, StrVal.Length);

            CipherDictionary = new List<CipherCtx>();

            for (var i = 0; i < StrVal.Length; i++)
            {
                var gg = new CipherCtx
                {
                    Text = StrVal[i],
                    Pass = ky[i]
                };

                GetCipher(gg, CipherMode);

                CipherDictionary.Add(gg);

            }

            return new string(CipherDictionary.Select(x => x.Cipher).ToArray());
        }

        public float Cipher(float intVal, float Key, Mode CipherMode = Mode.Encrypt)
        {

            var kys = ReGenerateKey(Key.ToString(), intVal.ToString().Length);

            var s = intVal.ToString();
            var g = "";

            for (var i = 0; i < s.Length; i++)
            {
                if (new string[] { ".", "-" }.Contains(s[i].ToString()))
                {
                    g += s[i].ToString();
                    continue;
                }

                var str = Convert.ToInt32(s[i].ToString());
                var kyr = Convert.ToInt32(kys[i].ToString());

                if (CipherMode == Mode.Encrypt)
                {
                    var j = NumDic[str, kyr];
                    g += j.ToString();
                }
                else
                { 
                    for (var o = 0; o <= 9; o++)
                    {
                        if (NumDic[kyr, o] == str)
                            g += o.ToString();
                    }
                }
            }

            return float.Parse(g);
        }

        private void GetCipher(CipherCtx ctx, Mode CipherMode)
        {
            if (!CharString.Any(x => x.Equals(ctx.Text)))
            {
                ctx.Cipher = ctx.Text;
                return;
            }

            ctx.TextIndex = CharString.IndexOf(ctx.Text);
            ctx.PassIndex = CharString.IndexOf(ctx.Pass);

            var gc = CipherMode == Mode.Encrypt ? ctx.TextIndex + ctx.PassIndex : ctx.TextIndex - ctx.PassIndex;

            if (gc > CharString.Length)
            {
                gc = gc - (CharString.Length - 1);
            }
            if (gc < 0)
                gc = (CharString.Length - 1) - Math.Abs(gc);

            ctx.CipherIndex = gc;
            ctx.Cipher = CharString[gc];
        }

        private string ReGenerateKey(string key, int lngth)
        {
            if (key.Length < lngth)
                return ReGenerateKey(key + key, lngth);
            else
                return key.Substring(0, lngth);
        }

    }

    public class CipherCtx
    {
        public char Text { get; set; }
        public char Pass { get; set; }
        public char Cipher { get; set; }

        public int TextIndex { get; set; }
        public int PassIndex { get; set; }
        public int CipherIndex { get; set; }
    }
}
