using google.protobuf;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class WorldServerLogic : NetServerBase
{
    public ConcurrentBag<Client> Gates = new ConcurrentBag<Client>();
    public Dictionary<string,Client> GameServers = new Dictionary<string, Client>();
    public WorldServerLogic(int _port)
    {
        base.Init(_port);
        ClientConn += WorldServerLogic_ClientConn;
    }

    private void WorldServerLogic_ClientConn(Client t1)
    {
        MainScript script = new MainScript();
        script.server = this;
        script.Init(t1);
        t1.Scripts.Add("main", script);
    }

    public void AddGameServer(Worlds.World world,Client client)
    {

    }
    public void RemoveGameServer(Worlds.World world)
    {

    }
}

