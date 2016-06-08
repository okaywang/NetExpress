using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetExpress.ResumeNumber
{
    public class ResumeNumbereEcrption
    {

        private static byte[] iv;
        private static byte[] key;

        static ResumeNumbereEcrption()
        {
            //Get iv and key from remote config.
            iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            key = Encoding.ASCII.GetBytes("zhaopin");

            //iv size:8
            if (iv.Length != 8)
            {
                throw new Exception("invalid iv");
            }

            //key size:5-16
            if (key.Length < 5 || key.Length > 16)
            {
                throw new Exception("invalid key");
            }
        }
        public static string Encrypt(long companyId, string resumeNumber)
        {
            var rawBytes = Translator.Translate((int)companyId, resumeNumber);
            var encrptedBytes = Encrypt(rawBytes);
            var str = GetEncodedString(encrptedBytes);
            return str;
        }
        public static string Decrypt(string encrptedString, out long companyId)
        {
            var encrptedBytes = GetBytesFromEncodedString(encrptedString);
            var rawBytes = Decrypt(encrptedBytes);
            int cId;
            var resumeNumber = Translator.Translate(rawBytes, out cId);
            companyId = (long)cId;
            return resumeNumber;
        }
        private static string GetEncodedString(byte[] bytes)
        {
            var str = Convert.ToBase64String(bytes);
            var result = str.Replace("+", "(");
            result = result.Replace("/", ")");
            result = result.Replace("=", "");
            return result;
        }

        private static byte[] GetBytesFromEncodedString(string str)
        {
            var result = str.Replace("(", "+");
            result = result.Replace(")", "/");
            var equalCount = str.Length % 4;
            for (int i = 0; i < equalCount; i++)
            {
                result += "=";
            }
            var bytes = Convert.FromBase64String(result);
            return bytes;
        }
        private static byte[] Encrypt(byte[] bytes)
        {

            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();

            ICryptoTransform enctyptor = rc2.CreateEncryptor(key, iv);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, enctyptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
        }

        private static byte[] Decrypt(byte[] bytes)
        {
            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
            ICryptoTransform dectyptor = rc2.CreateDecryptor(key, iv);
            using (MemoryStream ms = new MemoryStream())
            {
                var buffer = new byte[ms.Length];
                using (CryptoStream cs = new CryptoStream(ms, dectyptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }


        //class Translator0
        //{
        //    public static byte[] Translate(int companyId, string resumeNumber)
        //    {
        //        string str = companyId.ToString() + resumeNumber;
        //        return Encoding.UTF8.GetBytes(str);
        //    }
        //    public static string Translate(byte[] bytes, out int companyId)
        //    {
        //        var str = Encoding.UTF8.GetString(bytes);
        //        var index = str.IndexOf("J");
        //        companyId = int.Parse(str.Substring(0, index));
        //        var resumeNumber = str.Substring(index, str.Length - index);
        //        return resumeNumber;
        //    }
        //}

        class Translator
        {
            public static byte[] Translate(int companyId, string resumeNumber)
            {
                int userId = int.Parse(resumeNumber.Substring(2, 9));
                int id = int.Parse(resumeNumber.Substring(12, 9));
                var bytesCompanyId = BitConverter.GetBytes(companyId);
                var bytesUserId = BitConverter.GetBytes(userId);
                var bytesChar = new byte[] { (byte)(resumeNumber.Substring(1, 1)[0]) };
                var bytes2 = BitConverter.GetBytes(id);
                var bytes = bytesUserId.Concat(bytesChar).Concat(bytesCompanyId).Concat(bytes2).ToArray();
                return bytes;
            }
            public static string Translate(byte[] bytes, out int companyId)
            {
                var bytesUserId = bytes.Take(4).ToArray();
                var byteM = bytes.Skip(4).Take(1).ToArray();
                var bytesCompanyId = bytes.Skip(5).Take(4).ToArray();
                var byte2 = bytes.Skip(9).Take(4).ToArray();

                companyId = BitConverter.ToInt32(bytesCompanyId, 0);
                var userId = BitConverter.ToInt32(bytesUserId, 0);
                var ch = (char)(byteM[0]);
                var id = BitConverter.ToInt32(byte2, 0);
                return string.Format("J{0}{1:D9}R{2:D9}00", ch, userId, id);
            }
        }
    }
}



