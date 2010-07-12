using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Site;
using Gooeycms.Business.Crypto;

namespace Gooeycms.test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Hash.MD5("This is a test"));
            Console.WriteLine(Hash.MD5("This is a testa"));
            Console.WriteLine(Hash.MD5("This is a testb"));
            Console.Read();
        }
    }
}
