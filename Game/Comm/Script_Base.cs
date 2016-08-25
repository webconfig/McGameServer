using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Script_Base
{
    public Client client;
    public int Command;
    public virtual void Init(Client t1)
    {
        client = t1;
    }
    public virtual void End()
    {

    }

    public virtual void Copy(Script_Base data)
    {
        data.Command = this.Command;
    }
}