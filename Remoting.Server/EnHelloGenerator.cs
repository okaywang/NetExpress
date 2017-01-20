using Remoting.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.Server
{
    public class EnHelloGenerator : MarshalByRefObject, IHelloGenerator
    {
        public string GetHelloString(string name)
        {
            return String.Format("Hello, {0}", name);
        }
    }
}
