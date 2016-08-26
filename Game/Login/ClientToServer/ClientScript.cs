using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientScript : Script_Base
{
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.Actions.Add(1, ClientEvent);
        client.DisConnEvent += Client_DisConnEvent;
    }

    private void Client_DisConnEvent(Client _client)
    {
        Debug.Info("客户端断开");
    }

    public void ClientEvent(int commmand_local,Byte[] datas)
    {
        switch (commmand_local)
        {
            case 1:
                Debug.Info("客户端登陆");
                NetHelp.Send(110, App.ClientDatas, client._stream);
                break;
            case 2:
                Debug.Info("查询WorldServer信息");
                break;
        }
    }
}
