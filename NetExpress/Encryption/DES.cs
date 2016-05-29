using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetExpress.Encryption
{
    public class DES
    {
        public static byte[] Encrypt(byte[] bytes, byte[] key)
        {  
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                ICryptoTransform transform = des.CreateEncryptor(key, key);

                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);

                    cs.FlushFinalBlock();
                }
                return ms.ToArray();
            }
        }

        public static byte[] DesDecrypt(byte[] bytes, byte[] key)
        { 
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(key, key), CryptoStreamMode.Write);
            cStream.Write(bytes, 0, bytes.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }
    }
}
