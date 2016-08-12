using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class Client
{
    public TcpClient _client;
    public NetworkStream _stream;
    public List<Script_Base<Client>> Scripts;
    public Dictionary<int, CallBack<int, Byte[]>> Actions = new Dictionary<int, CallBack<int, byte[]>>();
    private List<byte> AddDatas;
    private List<byte> AllDatas;
    private byte[] recieveData;
    private Int32 ReceiveBufferSize = 5 * 1024;
    public System.DateTime StartTime;
    private int len = 0;
    private int command = 0;
    private int recv_count = 0;
    public NetServerBase Server;
    public event CallBack DisConnEvent;
    public Client(NetServerBase _Server,TcpClient client, List<Script_Base<Client>> _Scripts)
    {
        Server = _Server;
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
                item.Init(this);
            }
        }
    }
    public void close()
    {
        if (_stream != null)
        {
            _stream.Dispose();
            _stream = null;
        }
        if (_client!=null)
        {
            _client.Close();
            _client = null;
        }
        if(DisConnEvent!=null)
        {
            DisConnEvent();
        }
        Server.RemoveClient(this);
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
                    int command_script = command / 100;
                    int command_local = command % 100;
                    if (Actions.ContainsKey(command_script))
                    {
                        Actions[command_script](command_local, msgBytes);
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
    }
    public void LateUpdate()
    {
        //if(LateUpdateEvent!=null)
        //{
        //    LateUpdateEvent();
        //}
    }
    private int index = 0;
}

