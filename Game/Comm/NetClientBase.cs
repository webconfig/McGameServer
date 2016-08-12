using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetClientBase
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
    public List<Script_Base<NetClientBase>> Scripts;
    public Dictionary<int, CallBack<int, Byte[]>> Actions = new Dictionary<int, CallBack<int, byte[]>>();

    public event CallBack ConnEvent,DisConnEvent;
    public void Init(string _server_ip, int _port)
    {
        if (Scripts != null)
        {
            foreach (Script_Base<NetClientBase> sb in Scripts)
            {
                sb.Init(this);
            }
        }

        server_ip = _server_ip;
        port = _port;
        recieveData = new byte[ReceiveBufferSize];
        try
        {
            Debug.Info("==连接:" + server_ip + ":" + port);
            client = new TcpClient();
            client.Connect(server_ip, port);
            NetStream = client.GetStream();
            BeginRead();
            if (ConnEvent != null) { ConnEvent(); }
            State = NetClientState.Conn;
        }
        catch (Exception ex)
        {
            Debug.Error(ex.ToString());
            State = NetClientState.ConnFailStart;
            return;
        }
        TimeManager.Instance.Update += Update;
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
            BeginRead();
            if (ConnEvent != null) { ConnEvent(); }
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
        index = 0;
        do
        {
            if (AllDatas.Count > 7)
            {
                //读取消息体的长度
                NetHelp.BytesToInt(AllDatas, 0, ref len);
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
        }while (index < 10);
    }
    #endregion

    public  void Update()
    {
        if (!CanRun)
        {
            return;
        }
        CanRun = false;
        try
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
                if (DisConnEvent != null) { DisConnEvent(); }
            }
            else if (State == NetClientState.ConnFail)
            {//连接失败

            }
            else
            {
                DelRecvData();
            }
        }
        catch { }
        CanRun = true;
    }
    private bool CanRun = true;
    private int index = 0;
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

