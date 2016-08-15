using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldServerLogic : NetServerBase
{
    public List<Client> GameServers=new List<Client>();
    public List<Client> GateServers=new List<Client>();
    public WorldServerLogic(int _port)
    {
        Scripts = new List<Script_Base<Client>>();
        Script_Register sr = new Script_Register();
        sr.server = this;
        Scripts.Add(sr);
        base.Init(_port);
    }
}

