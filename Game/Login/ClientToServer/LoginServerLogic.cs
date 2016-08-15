using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoginServerLogic : NetServerBase
{
    public LoginServerLogic(int _port)
    {
        Scripts = new List<Script_Base<Client>>();
        Script_Login sr = new Script_Login();
        Scripts.Add(sr);
        base.Init(_port);
    }
}

