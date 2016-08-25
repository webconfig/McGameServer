using google.protobuf;
using System.Collections.Generic;

public class MasterServerLogic : NetServerBase
{
    public List<Client> LoginServers=new List<Client>();
    public Worlds AllWorld;
    public Dictionary<string,Client> WorldServers = new Dictionary<string, Client>();
    public MasterServerLogic(int _port)
    {
        base.Init(_port);
        AllWorld = new Worlds();
        ClientConn += MasterServerLogic_ClientConn;
    }

    private void MasterServerLogic_ClientConn(Client t1)
    {
        MainScript script = new MainScript();
        script.server = this;
        script.Init(t1);
        t1.Scripts.Add("main", script);
    }

    public void AddWorld(Worlds.World world,Client client)
    {
        AllWorld.value.Add(world);
        WorldServers.Add(world.Name, client);
    }
    public void RemoveWorld(Worlds.World world)
    {
        AllWorld.value.Remove(world);
        WorldServers.Remove(world.Name);
    }
}

