using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Script_Register : Script_Base<Client>, IDeepCopy<Client>
{
    public MasterServerLogic server;
    /// <summary>
    /// 1: Login 2:Gate
    /// </summary>
    public int type;
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.Actions.Add(1, ClientEvent);
        client.DisConnEvent += Client_DisConnEvent;
    }

    private void Client_DisConnEvent()
    {
        switch(type)
        {
            case 1:
                if (server.LoginServers.Contains(client))
                {
                    Debug.Info("断开连接-[登陆服务器]");
                    server.LoginServers.Remove(client);
                }
                break;
            case 2:
                if (server.GateServers.Contains(client))
                {
                    Debug.Info("断开连接-[网关服务器]");
                    server.GateServers.Remove(client);
                }
                break;
        }
    }

    public void ClientEvent(int commmand_local,Byte[] datas)
    {
        switch(commmand_local)
        {
            case 1:
                Debug.Info("接受注册--[登陆服务器]");
                NetHelp.Send(101, client._stream);
                type = 1;
                server.LoginServers.Add(client);
                break;
            case 2:
                Debug.Info("接受注册--[网关服务器]");
                NetHelp.Send(102, client._stream);
                type = 2;
                server.GateServers.Add(client);
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
        sr.server = server;
        sr.client = null;
        Copy(sr);
        return sr;
    }
    #endregion
}
