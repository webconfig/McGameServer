using System.Collections.Generic;

public class NetServer : NetClientBase
{
    public NetServer(string _server_ip, int _port):base(_server_ip, _port)
    {
        NetServerScript script = new NetServerScript();
        script.Init(client);
        client.Scripts.Add("register", script);
        ConnServer();
    }
}