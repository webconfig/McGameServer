using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetServerBase
{
    int port;
    private TcpListener NetworkListener;
    public List<Script_Base<Client>> Scripts;
    public List<Client> Clients = new List<Client>();

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
            AddClient(tcpClient);
        }
        catch { }
        NetworkListener.BeginAcceptTcpClient(new AsyncCallback(BeginAcceptTcpClient), null);
    }
    public void AddClient(TcpClient tcp)
    {
        List<Script_Base<Client>> _Scripts = null;
        if (Scripts != null)
        {
            _Scripts = new List<Script_Base<Client>>();
            foreach (var item in Scripts)
            {
                _Scripts.Add((item as IDeepCopy<Client>).DeepCopy());
            }
        }
        Client client = new Client(tcp, _Scripts);
        client.DisConnEvent += Client_DisConnEvent;
        Clients.Add(client);
    }

    private void Client_DisConnEvent(Client t1)
    {
        Clients.Remove(t1);
    }
}

