using System;
using System.Diagnostics;
namespace WJ_Server
{
    public class Program
    {
        static void Main()
        {
            GataToMaster ToMaster = new GataToMaster("127.0.0.1", 5001);
            Console.ReadLine();
        }
    }
}
