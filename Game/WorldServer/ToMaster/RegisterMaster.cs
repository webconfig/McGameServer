using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
public class RegisterMaster : Script_Base
{
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.ConnEvent += ConnEvent;
        client.Actions.Add(1, ActionEvent);
    }
    public void ConnEvent()
    {
        Worlds datas = new Worlds();
        Worlds.World item = new Worlds.World();
        item.Name=System.DateTime.Now.Ticks.ToString();
        datas.value.Add(item);
        int k = NetHelp.Send<Worlds>(120, datas, client._stream);
        if (k > 0)
        {
            Debug.Info("连接成功--〉开始注册");
            return;
        }
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
