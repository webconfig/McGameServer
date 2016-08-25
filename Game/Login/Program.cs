using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public  class Program
{
    public static LoginToMaster ToMaster;
    public static ClientToServer ClientServer;
    static void Main(string[] args)
    {
        ToMaster = new LoginToMaster("127.0.0.1", 5000);
        ClientServer = new ClientToServer(50001);
        Console.ReadLine();
    }
}
