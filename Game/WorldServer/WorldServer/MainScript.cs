using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainScript : Script_Base
{
    public WorldServerLogic server;
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.Actions.Add(1, ClientEvent);
    }

    public void ClientEvent(int commmand_local, Byte[] datas)
    {
        switch (commmand_local)
        {
            case 10://Gate服务器注册
                NetHelp.Send(101, client._stream);
                server.Gates.Add(client);
                Debug.Info("接受注册--[Gate服务器]");
                client.DisConnEvent += Gate_DisConnEvent;
                break;
            //case 20:
            //    if (!client.Scripts.ContainsKey("world"))
            //    {
            //        Worlds data;
            //        Worlds.World world;
            //        NetHelp.RecvData(datas, out data);
            //        world = data.value[0];
            //        NetHelp.Send(101, client._stream);
            //        server.AddWorld(world, client);
            //        Debug.Info(string.Format("接受注册--[WorldServer服务器]--[{0}]", world.Name));

            //        WorldScript world_script = new WorldScript();
            //        world_script.world = world;
            //        world_script.server = server;
            //        world_script.Init(client);
            //        client.Scripts.Add("world", world_script);
            //    }
            //    break;
        }
    }

    private void Gate_DisConnEvent(Client t1)
    {
        server.Gates.TryTake(out t1);
    }
}

