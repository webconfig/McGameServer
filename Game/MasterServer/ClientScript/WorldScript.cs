using google.protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldScript : Script_Base
{
    public MasterServerLogic server;
    public Worlds.World world;
    public override void Init(Client t1)
    {
        base.Init(t1);
        client.DisConnEvent += Client_DisConnEvent;
    }

    private void Client_DisConnEvent(Client t1)
    {
        Debug.Info(string.Format("[WorldServer服务器]--[{0}]--断线", world.Name));
        server.RemoveWorld(world);
    }
}

