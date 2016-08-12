using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Script_Base<T>
{
    public T client;
    public int Command;
    public virtual void Init(T t1)
    {
        client = t1;
    }
    public virtual void End()
    {

    }

    public virtual void Copy(Script_Base<T> data)
    {
        data.Command = this.Command;
    }
}