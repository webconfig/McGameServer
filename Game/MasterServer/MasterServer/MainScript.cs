using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainScript : Script_Base
{
    public MasterServerLogic server;
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.Actions.Add(1, ClientEvent);
    }

    public void ClientEvent(int commmand_local, Byte[] datas)
    {
        switch (commmand_local)
        {
            case 10://登陆服务器注册
                if (!client.Scripts.ContainsKey("login"))
                {
                    NetHelp.Send(101, client._stream);
                    server.LoginServers.Add(client);
                    Debug.Info("接受注册--[登陆服务器]");

                    LoginScript login_script = new LoginScript();
                    login_script.server = server;
                    login_script.Init(client);
                    client.Scripts.Add("login", login_script);
                }
                break;
            case 11://登陆服务器获取worldserver
                NetHelp.Send(102, server.AllWorld, client._stream);
                break;
            case 20:
                if(!client.Scripts.ContainsKey("world"))
                {
                    Worlds data;
                    Worlds.World world;
                    NetHelp.RecvData(datas, out data);
                    world = data.value[0];
                    NetHelp.Send(101, client._stream);
                    server.AddWorld(world, client);
                    Debug.Info(string.Format("接受注册--[WorldServer服务器]--[{0}]", world.Name));

                    WorldScript world_script = new WorldScript();
                    world_script.world = world;
                    world_script.server = server;
                    world_script.Init(client);
                    client.Scripts.Add("world", world_script);
                }
                break;
        }
    }
}

