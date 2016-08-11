using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetClientBase : Action_Base
{
    public int port;
    public string server_ip;
    public static TcpClient client;
    public NetworkStream NetStream;
    public NetClientState State;
    private byte[] recieveData;
    private Int32 ReceiveBufferSize = 3 * 1024;
    private List<byte> AddDatas = new List<byte>();
    private List<byte> AllDatas = new List<byte>();
    private bool CanAdd = true;
    private int len = 0;
    private int command = 0;
    public NetClientBase(string _server_ip, int _port)
    {
        server_ip = _server_ip;
        port = _port;
        recieveData = new byte[ReceiveBufferSize];
        try
        {
            Debug.Info("==连接:" + server_ip + ":" + port);
            client.Connect(server_ip, port);
            State = NetClientState.Conn;
        }
        catch
        {
            State = NetClientState.ConnFailStart;
            return;
        }
    }
    /// <summary>
    /// 重连
    /// </summary>
    private void ConnServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(server_ip, port);
            NetStream = client.GetStream();
            State = NetClientState.Conn;
            BeginRead();
            State = NetClientState.Conn;
        }
        catch
        {
            return;
        }
    }

    #region 接受数据
    private void BeginRead()
    {
        try
        {
            if (this.NetStream == null || !this.NetStream.CanRead)
                return;
            NetStream.BeginRead(recieveData, 0, ReceiveBufferSize, ReceiveMsg, null);
        }
        catch
        {
            State = NetClientState.ConnFailStart;
        }
    }
    public void ReceiveMsg(IAsyncResult ar)//异步接收消息
    {
        int length = 0;
        try
        {
            length = NetStream.EndRead(ar);
        }
        catch
        {
            State = NetClientState.ConnFailStart;
            return;
        }
        if (length == 0)
        {
            State = NetClientState.ConnFailStart;
            return;
        }
        else if (length > 0)
        {
            CanAdd = false;
            //拷贝到缓存队列
            for (int i = 0; i < length; i++)
            {
                AddDatas.Add(recieveData[i]);
            }
            CanAdd = true;
        }
        BeginRead();
    }
    /// <summary>
    /// 处理数据
    /// </summary>
    public void DelRecvData()
    {
        if (CanAdd)
        {
            if (AddDatas.Count > 0)
            {
                AllDatas.AddRange(AddDatas);
                AddDatas.Clear();
            }
        }
        if (AllDatas.Count > 0)
        {
            //读取消息体的长度
            NetHelp.BytesToInt(AllDatas, 0,ref len);
            len += 4;
            //读取消息体内容
            if (len + 4 <= AllDatas.Count)
            {
                NetHelp.BytesToInt(AllDatas, 4,ref command);//操作命令
                byte[] msgBytes = new byte[len - 4];
                AllDatas.CopyTo(8, msgBytes, 0, msgBytes.Length);
                AllDatas.RemoveRange(0, len + 4);
            }
        }
    }
    #endregion

    public override void Update()
    {
        if (State == NetClientState.ConnFailStart)
        {
            State = NetClientState.ConnFail;
            if (NetStream != null)
            {
                NetStream.Close();
                NetStream = null;
            }
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }
        else if (State == NetClientState.ConnFail)
        {//连接失败

        }
        else
        {
            DelRecvData();
        }
    }
}
public enum NetClientState
{
    ConnFailStart,
    ConnFail,
    Conn,
    ReLogin,
    Logining,
    LoginBack,
    LoginFail,
    LoingOk,
    Sending,
}

