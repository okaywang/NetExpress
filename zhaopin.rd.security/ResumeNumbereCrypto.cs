using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zhaopin.RD.Security
{

    public class ResumeNumbereCrypto
    {
        private RC2CryptoServiceProvider rc2;
        private ICryptoTransform enctyptor;
        private ICryptoTransform dectyptor;

        private Regex resumeNumberRegex = new Regex(@"^J\w\d{9}R\d{9}00$");
        private byte[] key;
        private byte[] iv;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iv">8位</param>
        /// <param name="key">5-16位</param>
        public ResumeNumbereCrypto(byte[] iv, byte[] key)
        {
            //iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            //key = Encoding.ASCII.GetBytes("zhaopin");

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

            rc2 = new RC2CryptoServiceProvider();
            //this.key = key;
            //this.iv = iv;

            enctyptor = rc2.CreateEncryptor(key, iv);
            dectyptor = rc2.CreateDecryptor(key, iv);
        }

        /// <summary>
        /// 简历编号加密
        /// </summary>
        /// <param name="rootCompanyId">总公司Id</param>
        /// <param name="resumeNumber">简历编号</param>
        /// <returns></returns>
        public string Encrypt(long rootCompanyId, string resumeNumber)
        {
            int rootId = checked((int)rootCompanyId);
            if (resumeNumber == null || !resumeNumberRegex.IsMatch(resumeNumber))
            {
                throw new Exception("invalid resumeNumber");
            }

            var rawBytes = Translator.Translate((int)rootId, resumeNumber);
            var encrptedBytes = Encrypt(rawBytes);
            var str = GetEncodedString(encrptedBytes);
            return str;
        }

        /// <summary>
        /// 简历编号解密
        /// </summary>
        /// <param name="encrptedString">加密后的字符串</param>
        /// <param name="rootCompanyId">总公司Id</param>
        /// <returns></returns>
        public string Decrypt(string encrptedString, out long rootCompanyId)
        {
            var encrptedBytes = GetBytesFromEncodedString(encrptedString);
            var rawBytes = Decrypt(encrptedBytes);
            int cId;
            var resumeNumber = Translator.Translate(rawBytes, out cId);
            rootCompanyId = (long)cId;
            return resumeNumber;
        }
        private string GetEncodedString(byte[] bytes)
        {
            var str = Convert.ToBase64String(bytes);
            var result = str.Replace("+", "(");
            result = result.Replace("/", ")");
            result = result.Replace("=", "");
            return result;
        }

        private byte[] GetBytesFromEncodedString(string str)
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
        private byte[] Encrypt(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //var enctyptor = rc2.CreateEncryptor(key, iv);
                using (CryptoStream cs = new CryptoStream(ms, enctyptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private byte[] Decrypt(byte[] bytes)
        {
            //lock (dectyptor)
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    //var dectyptor = rc2.CreateDecryptor(key, iv);

                    using (CryptoStream cs = new CryptoStream(ms, dectyptor, CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
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
