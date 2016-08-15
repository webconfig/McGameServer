using System.Collections.Generic;

public class WorldServerToMaster : NetClientBase
{
    public WorldServerToMaster(string _server_ip, int _port)
    {
        script = new List<Script_Base<Client>>();
        script.Add(new WorldServerToMaster_Register());
        Init(_server_ip, _port);
    }
}

