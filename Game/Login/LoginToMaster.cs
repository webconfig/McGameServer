using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoginToMaster : NetClientBase
{
    public LoginToMaster(string _server_ip, int _port)
    {
        Scripts = new List<Script_Base<NetClientBase>>();
        Scripts.Add(new Script_Register());
        Init(_server_ip, _port);
    }
}

