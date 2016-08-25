using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
public class NetServerBase
{
    int port;
    private TcpListener NetworkListener;
    public ConcurrentBag<Client> Clients = new ConcurrentBag<Client>();
    public event CallBack<Client> ClientConn;
    public void Init(int _port)
    {
        port = _port;
        new Thread(new ThreadStart(NetworkStart)).Start();
    }
    private void NetworkStart()
    {
        try
        {
            NetworkListener = new TcpListener(new System.Net.IPEndPoint(0, port));
            NetworkListener.Start();
            Debug.Info(string.Format("Server listening clients at {0}:{1}...", ((IPEndPoint)NetworkListener.LocalEndpoint).Address, port));
            NetworkListener.BeginAcceptTcpClient(new AsyncCallback(BeginAcceptTcpClient), null);
        }
        catch (Exception ex)
        {
            Debug.Error("NetworkStart:" + ex);
        }
    }
    private void BeginAcceptTcpClient(IAsyncResult ar)
    {
        try
        {
            TcpClient tcpClient = NetworkListener.EndAcceptTcpClient(ar);
            Client client = new Client(tcpClient);
            client.DisConnEvent += Client_DisConnEvent;
            Clients.Add(client);
            if (ClientConn != null) { ClientConn(client); }
        }
        catch { }
        NetworkListener.BeginAcceptTcpClient(new AsyncCallback(BeginAcceptTcpClient), null);
    }
    private void Client_DisConnEvent(Client t1)
    {
        Clients.TryTake(out t1);
    }
}

