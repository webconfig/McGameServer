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
    public List<Script_Base<Client>> script;
    public TcpClient tcp_client;
    public Client client;
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
        }
        catch (Exception ex)
        {
            Debug.Error(ex.ToString());
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

