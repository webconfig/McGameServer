using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
public class NetClientBase
{
    private int port;
    private string server_ip;
    public Client client;
    public int ReConnTime = 2, ReConnTimeMax=20, ReConnTimeAdd=2;
    public  NetClientBase(string _server_ip, int _port)
    {
        server_ip = _server_ip;
        port = _port;
        client = new Client();
    }
    public bool ConnServer()
    {
        try
        {
            TcpClient tcp_client = new TcpClient();
            Debug.Info("==连接:" + server_ip + ":" + port);
            tcp_client.Connect(server_ip, port);
            client.Init(tcp_client);
            ReConnTime = 0;
            client.CallConnEvent();
            return true;
        }
        catch (Exception ex)
        {
            //Debug.Error(ex.ToString());
            return false;
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

