using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Cipher
{
    public interface IBlockCipher
    {
        void InitCipher(byte[] key); 
        void Cipher(byte[] inb, byte[] outb);
        void InvCipher(byte[] inb, byte[] outb);
        int[] KeySizesInBytes();
       
        int BlockSizeInBytes();
    }//EOC
}
