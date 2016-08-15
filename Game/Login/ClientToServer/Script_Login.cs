using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Script_Login : Script_Base<Client>, IDeepCopy<Client>
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
                NetHelp.Send(101, client._stream);
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
        Script_Login sr = new Script_Login();
        sr.client = null;
        Copy(sr);
        return sr;
    }
    #endregion
}
