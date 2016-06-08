using NetExpress.Compression;
using NetExpress.Encryption;
using NetExpress.Http;
using NetExpress.ResumeNumber;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetExpress
{
    class Program
    {

        static byte[] key2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static void Main(string[] args)
        {

            var bytes = new Byte[] {1 };
            var x = Convert.ToBase64String(bytes);
            var y = Convert.FromBase64String(x);

            long companyId = 13412341;
            string resumeNumber = "JM003220085R00250000000";
            var encrptedString = ResumeNumbereEcrption.Encrypt(companyId, resumeNumber);

            //--------------------------------------------------------

            long cId;
            var result = ResumeNumbereEcrption.Decrypt(encrptedString, out cId);
            Console.WriteLine(result == resumeNumber);

        }
        public static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        private static void NewMethod()
        {
            int id = 12007036;
            int ii = 2147483647;
            //var resumeNumber = "JR280497991R90250001000";
            var resumeNumber = "JM403515245R90250000000";
            long x = 2804979990250001000;

            var bb = new List<byte>();
            bb.AddRange(BitConverter.GetBytes(12007036));
            bb.AddRange(BitConverter.GetBytes(12007036));


            byte[] key = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            var bytes = Encoding.ASCII.GetBytes("JM403515245R90250000000");
            var bytes2 = Encoding.ASCII.GetBytes("1234567890abc");
            var xx = AES.Encrypt(bytes2, key);




            var key2 = BitConverter.GetBytes(33333l);
            var rrr = DES.Encrypt(bytes, key2);
            var xx1 = DES.DesDecrypt(rrr, key2);

            var eee = RSA1.Encrypt(bytes2);
            var r44444 = RSA1.Decrypt(eee);

            Print(bytes);
        }
        static void Print(byte[] bytes)
        {
            Console.WriteLine(string.Format("{0},{1}", bytes.Length, BitConverter.ToString(bytes)));
        }
        static byte[] getBytes1(long rootId, string resumeNumber)
        {
            var bytes = Encoding.UTF8.GetBytes(rootId.ToString() + resumeNumber.ToString());
            return bytes;
        }
    }
}
