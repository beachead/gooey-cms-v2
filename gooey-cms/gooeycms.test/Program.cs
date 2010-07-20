using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;

namespace Gooeycms.test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TextHash.MD5("This is a test"));
            Console.WriteLine(TextHash.MD5("This is a testa"));
            Console.WriteLine(TextHash.MD5("This is a testb"));
            Console.Read();
        }
    }
}
