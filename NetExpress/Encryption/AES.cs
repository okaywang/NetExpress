using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetExpress.Encryption
{
    public class AES
    {
        public static byte[] Encrypt(byte[] toEncrypt, byte[] key)
        {
            
            //byte[] key = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

            return resultArray;
        }

        public static string Decrypt(byte[] toDecrypt, byte[] key)
        {
            //byte[] key = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012"); 
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
