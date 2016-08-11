using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class Client
{
    public TcpClient _client;
    public NetworkStream _stream;
    public Dictionary<int, Script_Base> Scripts;
    private List<byte> AddDatas;
    private List<byte> AllDatas;
    private bool CanRun = true;
    private byte[] recieveData;
    private Int32 ReceiveBufferSize = 5 * 1024;
    public System.DateTime StartTime;
    private int len = 0;
    private int command = 0;
    private int recv_count = 0;
    public Client(TcpClient client, Dictionary<int, Script_Base> _Scripts)
    {
        _client = client;
        _stream = client.GetStream();
        AllDatas = new List<byte>();
        AddDatas = new List<byte>();
        recieveData = new byte[ReceiveBufferSize];
        StartTime = System.DateTime.Now;
        BeginRead();

        Scripts = _Scripts;
        if (Scripts!=null)
        {
            foreach(var item in Scripts)
            {
                item.Value.Init(this);
            }
        }
    }

    public void Disable()
    {
        Debug.Info("Client--Disable");
        if (_stream != null)
        {
            this._stream.Dispose();
            this._stream = null;
        }
    }

    public void close()
    {
        //Debug.Info("【Client】--被动关闭");
        Disable();
        //ClientManager.GetInstance().RemoveClient(this);
    }

    private void BeginRead()
    {
        try
        {
            if (this._stream == null || !this._stream.CanRead)
                return;
            _stream.BeginRead(recieveData, 0, ReceiveBufferSize, new AsyncCallback(OnReceiveCallback), null);
        }
        catch (Exception ex)
        {
            Debug.Error("[Client]: BeginRead() Exception" + ex);
            close();
        }
    }

    private void OnReceiveCallback(IAsyncResult ar)
    {
        //Debug.Info("【Client】--接收数据");
        int length = 0;
        try
        {
            length = _stream.EndRead(ar);
        }
        catch
        {
            close();
            return;
        }
        if (length == 0)
        {
            Debug.Error("接收数据长度：" + length);
            close();
            return;
        }
        else if (length > 0)
        {
            recv_count++;
            try
            {
                //拷贝到缓存队列
                for (int i = 0; i < length; i++)
                {
                    AddDatas.Add(recieveData[i]);
                }
            }
            catch { }
            recv_count--;
        }
        BeginRead();
    }

    public void Update()
    {
        if (!CanRun)
        {
            return;
        }
        CanRun = false;
        if (recv_count==0)
        {
            if (AddDatas.Count > 0)
            {
                AllDatas.AddRange(AddDatas);
                AddDatas.Clear();
            }
        }
        //==================
        index = 0;
        do
        {
            if (AllDatas.Count > 7)//最小的包应该有8个字节
            {
                NetHelp.BytesToInt(AllDatas, 0, ref len);//读取消息体的长度
                len += 4;
                //读取消息体内容
                if (len <= AllDatas.Count)
                {
                    NetHelp.BytesToInt(AllDatas, 4, ref command);//操作命令
                    byte[] msgBytes = new byte[len - 8];
                    AllDatas.CopyTo(8, msgBytes, 0, msgBytes.Length);
                    AllDatas.RemoveRange(0, len);
                    if (command == 0)
                    {
                        Debug.Info("相应心跳");
                    }
                    int command_script = command / 100;
                    int command_local = command % 100;
                    if (Scripts.ContainsKey(command_script))
                    {
                        Scripts[command_script].ScriptAction(command_local, msgBytes);
                    }
                    index++;
                }
                else
                {
                    break;
                }

            }
            else
            {
                break;
            }
        } while (index < 10);
        CanRun = true;
    }
    private int index = 0;
}

