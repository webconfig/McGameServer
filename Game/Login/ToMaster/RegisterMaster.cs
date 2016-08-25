
using google.protobuf;
using System;

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
        int k = NetHelp.Send(110, client._stream);
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
                NetHelp.Send(111, client._stream);
                break;
            case 2:
                Debug.Info("获取world成功");
                NetHelp.RecvData(datas, out App.datas);
                break;
        }
    }
}
