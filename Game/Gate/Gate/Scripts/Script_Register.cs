using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
public class Script_Register : Script_Base<Client>, IDeepCopy<Client>
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

        int total = 0, k = 0;
        for (int i = 0; i < 100; i++)
        {
            System.Threading.Thread.Sleep(10);
            k = NetHelp.Send(102, client._stream);
            if (k < 0) { return; }
            else
            {
                total += k;
            }
        }
        Debug.Info("发生数据长度：" + total);
    }
    public void ActionEvent(int command_local,Byte[] datas)
    {
        switch(command_local)
        {
            case 2:
                Debug.Info("注册成功");
                break;
        }
    }
    #region 拷贝
    public override void Copy(Script_Base<Client> data)
    {
        base.Copy(data);
    }
    public Script_Base<Client> DeepCopy()
    {
        Script_Register sr = new Script_Register();
        sr.client = null;
        Copy(sr);
        return sr;
    }
    #endregion
}
