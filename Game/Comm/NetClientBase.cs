using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
public class NetClientBase
{
    public int port;
    public string server_ip;
    public List<Script_Base<Client>> script;
    public TcpClient tcp_client;
    public Client client;
    private int ReConnTime = 2, ReConnTimeMax=20, ReConnTimeAdd=2;
    public void Init(string _server_ip, int _port)
    {
        server_ip = _server_ip;
        port = _port;
        ConnServer(null);
    }
    private void ConnServer(Client _client)
    {
        try
        {
            tcp_client = new TcpClient();
            Debug.Info("==连接:" + server_ip + ":" + port);
            tcp_client.Connect(server_ip, port);
            client = new Client(tcp_client, script);
            client.DisConnEvent += ConnServer;
            client.CallConnEvent();
            ReConnTime = 0;
            return;
        }
        catch (Exception ex)
        {
            Debug.Error(ex.ToString());
            Debug.Info(ReConnTime + "秒后重新连接服务器");
            Thread.Sleep(ReConnTime*1000);
            ReConnTime += ReConnTimeAdd;
            if (ReConnTime > ReConnTimeMax) { ReConnTime = ReConnTimeMax; }
            ConnServer(null);
            return;
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

