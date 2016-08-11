using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetServerBase : Action_Base
{
    int port;
    public NetServerBase(int _port)
    {
        port = _port;
        new Thread(new ThreadStart(NetworkStart)).Start();
        TimeManager.Instance.TimeAction += Update;
    }
    private TcpListener NetworkListener;
    private void NetworkStart()
    {
        try
        {
            NetworkListener = new TcpListener(new System.Net.IPEndPoint(0, port));
            NetworkListener.Start();
            Debug.Info(string.Format("Server listening clients at {0}:{1}...", ((IPEndPoint)NetworkListener.LocalEndpoint).Address, port));
            NetworkListener.BeginAcceptTcpClient(new AsyncCallback(BeginAcceptTcpClient), (object)null);
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
        catch
        {

        }
        NetworkListener.BeginAcceptTcpClient(new AsyncCallback(BeginAcceptTcpClient), null);
    }
    public Dictionary<int, Script_Base> Scripts;
    public List<Client> Clients = new List<Client>();
    public List<Client> Clients_Add = new List<Client>();
    private int AddCount = 0;
    public void AddClient(TcpClient tcp)
    {
        AddCount++;
        try
        {
            Dictionary<int, Script_Base> _Scripts = new Dictionary<int, Script_Base>();
            foreach(var item in Scripts)
            {
                _Scripts.Add(item.Key, (item.Value as IDeepCopy).DeepCopy());
            }
            Client client = new Client(tcp, _Scripts);
            Clients_Add.Add(client);
        }
        catch { }
        AddCount--;
    }

    public override void Update()
    {
        //===添加新的===
        if (AddCount == 0)
        {
            if (Clients_Add.Count > 0)
            {
                Clients.AddRange(Clients_Add);
                Clients_Add.Clear();
            }
        }
        //===执行逻辑===
        for (int i = 0; i < Clients.Count; i++)
        {
            Clients[i].Update();
        }
    }

}

