using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
public class RegisterWorldServer : Script_Base
{
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.ConnEvent += ConnEvent;
        client.Actions.Add(1, ActionEvent);
    }
    public void ConnEvent()
    {
        Debug.Info("连接成功--〉开始注册");
        int k = NetHelp.Send(110, client._stream);
    }
    public void ActionEvent(int command_local,Byte[] datas)
    {
        switch(command_local)
        {
            case 1:
                Debug.Info("注册成功");
                break;
        }
    }
}
