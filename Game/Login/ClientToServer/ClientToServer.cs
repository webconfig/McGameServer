using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientToServer : NetServerBase
{
    public ClientToServer(int _port)
    {
        base.Init(_port);
        ClientConn += ClientToServer_ClientConn;
    }

    private void ClientToServer_ClientConn(Client t1)
    {
        ClientScript script = new ClientScript();
        script.Init(t1);
        t1.Scripts.Add("main", script);
    }
}