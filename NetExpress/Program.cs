using NetExpress.Compression;
using NetExpress.Encryption;
using System;
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
            int id = 12007036;
            int ii = 2147483647;
            //var resumeNumber = "JR280497991R90250001000";
            var resumeNumber = "JM403515245R90250000000";
            long x = 2804979990250001000;



            byte[] key = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            var bytes = Encoding.ASCII.GetBytes("JM403515245R90250000000");
            var xx = AES.Encrypt(bytes, key);


            var key2 = BitConverter.GetBytes(33333l);
            var rrr = DES.Encrypt(bytes, key2);
            var xx1 = DES.DesDecrypt(rrr, key2);

            var eee = RSA1.Encrypt(key2);
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
