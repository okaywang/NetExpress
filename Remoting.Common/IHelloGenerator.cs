using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.Common
{
    public interface IHelloGenerator
    {
        string GetHelloString(string name);
    }
}
