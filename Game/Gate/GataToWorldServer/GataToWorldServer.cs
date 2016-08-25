using System.Collections.Generic;

public class GataToWorldServer : NetClientBase
{
    public GataToWorldServer(string _server_ip, int _port)
    {
        script = new List<Script_Base>();
        script.Add(new Script_Register());
        Init(_server_ip, _port);
    }
}

