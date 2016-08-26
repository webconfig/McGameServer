using System.Collections.Generic;
using System.Threading;

public class GataToWorldServer : NetClientBase
{

    public GataToWorldServer(string _server_ip, int _port):base(_server_ip, _port)
    {
        RegisterWorldServer script = new RegisterWorldServer();
        script.Init(client);
        client.Scripts.Add("register", script);
        if (!ConnServer())
        {
            ReConnServer();
        }
    }
    public void ReConnServer()
    {
        if (!ConnServer())
        {
            Thread.Sleep(ReConnTime * 1000);
            ReConnTime += ReConnTimeAdd;
            if (ReConnTime > ReConnTimeMax) { ReConnTime = ReConnTimeMax; }
            ReConnServer();
        }
    }
}