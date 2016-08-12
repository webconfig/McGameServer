using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginToMaster ToMaster = new LoginToMaster("127.0.0.1", 5000);
            Console.ReadLine();
        }
    }
}
