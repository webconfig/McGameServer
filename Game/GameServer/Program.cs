using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    static void Main()
    {
        GameServerToWorldServer ToWorldServer = new GameServerToWorldServer("127.0.0.1", 5100);
        Console.ReadLine();
    }
}
