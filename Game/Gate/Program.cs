using System;
using System.Diagnostics;
namespace WJ_Server
{
    public class Program
    {
        static void Main()
        {
            GataToWorldServer ToWorldServer = new GataToWorldServer("127.0.0.1", 5100);
            Console.ReadLine();
        }
    }
}
