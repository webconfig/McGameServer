using System.Collections.Generic;

public class GataToMaster : NetClientBase
{
    public GataToMaster(string _server_ip, int _port)
    {
        script = new List<Script_Base<Client>>();
        script.Add(new Script_Register());
        Init(_server_ip, _port);
    }
}

