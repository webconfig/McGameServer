using System.Collections.Generic;

public class NetServer : NetClientBase
{
    public NetServer(string _server_ip, int _port)
    {
        script = new List<Script_Base<Client>>();
        script.Add(new NetServerScript());
        Init(_server_ip, _port);
    }
}

