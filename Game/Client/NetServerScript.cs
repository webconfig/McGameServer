using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetServerScript : Script_Base
{
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.ConnEvent += ConnEvent;
        client.Actions.Add(1, ActionEvent);
    }
    public void ConnEvent()
    {
        Debug.Info("连接成功--〉开始登陆");
        NetHelp.Send(101, client._stream);
    }
    public void DisConnEvent()
    {
        Debug.Info("断开连接");
    }
    public void ActionEvent(int command_local, Byte[] datas)
    {
        switch (command_local)
        {
            case 10:
                NetHelp.RecvData(datas, out App.datas);
                Debug.Info("登陆成功,返回WorldServer："+ App.datas.value[0].Name);
                break;
        }
    }
}
