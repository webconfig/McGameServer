using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginToMaster LoginServer = new LoginToMaster(5000);
            GateToMaster GateServer = new GateToMaster(5001);
            Console.ReadLine();
        }
    }
}
