using Remoting.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //建立连接
            var serverProvider = new BinaryServerFormatterSinkProvider();
            var clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 999;
            HttpChannel channel = new HttpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(channel);

            //创建远程对象
            string RemoteServerUrl = "http://192.168.0.105:520/HelloGenerator.soap";
            var generator = (IHelloGenerator)Activator.GetObject(typeof(IHelloGenerator), RemoteServerUrl);
            Console.WriteLine(generator.GetHelloString("zhenyulu"));
            Console.Read();
        }
    }
}
