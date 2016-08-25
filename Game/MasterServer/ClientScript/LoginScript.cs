using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoginScript : Script_Base
{
    public MasterServerLogic server;
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.DisConnEvent += Client_DisConnEvent;
        //client.Actions.Add(1, ClientEvent);
    }

    private void Client_DisConnEvent(Client t1)
    {
        Debug.Info("[登陆服务器服务器]--断线");
        if (server.LoginServers.Contains(client))
        {
            server.LoginServers.Remove(client);
        }
    }

    //public void ClientEvent(int commmand_local, Byte[] datas)
    //{
    //    switch (commmand_local)
    //    {
    //        case 12://请求所有的world服务器
    //            NetHelp.Send(102, server.AllWorld, client._stream);
    //            break;
           
    //    }
    //}
}

