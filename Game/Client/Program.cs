﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        NetServer ToMaster = new NetServer("127.0.0.1", 50001);
        Console.ReadLine();
    }
}

