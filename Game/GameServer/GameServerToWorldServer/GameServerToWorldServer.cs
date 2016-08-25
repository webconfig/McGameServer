using System.Collections.Generic;

public class GameServerToWorldServer : NetClientBase
{
    public GameServerToWorldServer(string _server_ip, int _port)
    {
        script = new List<Script_Base>();
        script.Add(new Script_Register());
        Init(_server_ip, _port);
    }
}

