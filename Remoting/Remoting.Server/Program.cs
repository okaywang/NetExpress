using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 520;
            try
            {
                var serverProvider = new BinaryServerFormatterSinkProvider();
                var clientProvider = new BinaryClientFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                props["port"] = port;
                props["timeout"] = 2000;
                HttpChannel channel = new HttpChannel(props, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(
                   typeof(EnHelloGenerator),
                   "HelloGenerator.soap",
                   WellKnownObjectMode.Singleton);
                Console.WriteLine("Server started!\r\nPress ENTER key to stop the server...");
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("Server Start Error!");
            }
        }
    }
}
