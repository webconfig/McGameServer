using System.Collections.Generic;
using System.Threading;

public class LoginToMaster : NetClientBase
{
    
    public LoginToMaster(string _server_ip, int _port):base(_server_ip, _port)
    {
        RegisterMaster script = new RegisterMaster();
        script.Init(client);
        client.Scripts.Add("register", script);
        if(!ConnServer())
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