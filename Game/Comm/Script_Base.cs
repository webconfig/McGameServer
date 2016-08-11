using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Script_Base
{
    public int Command;
    public Client client;
    public virtual void Init(Client _client)
    {
        client = _client;
    }
    public virtual void ScriptAction(int CommandLoacl, byte[] datas)
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void End()
    {

    }

    public virtual void Copy(Script_Base data)
    {
        data.Command = this.Command;
        data.client = null;
    }
}