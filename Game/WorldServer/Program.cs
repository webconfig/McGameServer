﻿using System;

class Program
{
    static void Main(string[] args)
    {
        WorldServerLogic ServerLogic = new WorldServerLogic(5100);
        WorldServerToMaster LoginServer = new WorldServerToMaster("127.0.0.1", 5000);
        Console.ReadLine();
    }
}

