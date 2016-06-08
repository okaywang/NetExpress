
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zhaopin.RD.Security;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {

                //new Thread(() =>
                //{
                //    var controller1 = new MyController();
                //    var r = controller1.DecodeResumeNumbers("WayXWCGYGh0svh7O6K50gg");
                //    Console.WriteLine(r);
                //}).Start();

                //new Thread(() =>
                //{
                    var controller2 = new MyController();
                    var r2 = controller2.DecodeResumeNumbers("WayXWCGYGh0svh7O6K50gg,WayXWCGYGh0svh7O6K50gg");
                    Console.WriteLine(r2);
                //}).Start();

            }
            System.Threading.Thread.Sleep(30000);
        }
        private static void NewMethod()
        {

            var iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 2 };
            var key = Encoding.ASCII.GetBytes("zhaopin");
            var crpto = new ResumeNumbereCrypto(iv, key);


            long companyId = 2342134;
            string resumeNumber = "JM701524499R90250400000";
            var encrptedResumeNumber = crpto.Encrypt(companyId, resumeNumber);

            //--------------------------------------------------------

            long rootId;
            var decrptedResumeNumber = crpto.Decrypt(encrptedResumeNumber, out rootId);
            Console.WriteLine(decrptedResumeNumber == resumeNumber);
        }
    }

    class MyController
    {
        private static byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 2 };
        private static byte[] key = Encoding.ASCII.GetBytes("zhaopin");

        private static ResumeNumbereCrypto crpto = new ResumeNumbereCrypto(iv, key);
        static MyController()
        {

        }

        public string DecodeResumeNumbers(string resumeNumbers, string seperator = ",")
        {
            var results = new List<ResumeNumerSecurityInfo>();
            if (resumeNumbers != null)
            {
                //var crpto = new ResumeNumbereCrypto(iv, key);
                var items = resumeNumbers.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in items)
                {
                    long rootId;
                    var info = new ResumeNumerSecurityInfo();
                    info.OriginalCode = item;
                    try
                    {
                        info.ResumeNumber = crpto.Decrypt(item, out rootId);
                        info.RootCompanyId = rootId;
                    }
                    catch (Exception ex)
                    {
                        // Logger.Log("DecodeResumeNumbers " + item, ex.Message);
                    }
                    results.Add(info);
                }
            }

            var result = string.Join(seperator, results.Select(i => i.ToString()).ToArray());
            return result;
        }

        public class ResumeNumerSecurityInfo
        {
            public long RootCompanyId { get; set; }

            public string ResumeNumber { get; set; }

            public string OriginalCode { get; set; }

            public override string ToString()
            {
                return string.Format("{0}|{1}|{2}", ResumeNumber, RootCompanyId, OriginalCode);
            }
        }

    }
}
