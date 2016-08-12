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
    public List<Client> Clients_Add = new List<Client>();
    public List<Client> Clients_Remove = new List<Client>();
    private int AddCount = 0;
    private int RemoveCount = 0;

    public void Init(int _port)
    {
        port = _port;
        new Thread(new ThreadStart(NetworkStart)).Start();
        TimeManager.Instance.Update += Update;
    }
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
    public void AddClient(TcpClient tcp)
    {
        AddCount++;
        try
        {
            List<Script_Base<Client>> _Scripts=null;
            if (Scripts != null)
            {
                _Scripts = new List<Script_Base<Client>>();
                foreach (var item in Scripts)
                {
                    _Scripts.Add((item as IDeepCopy<Client>).DeepCopy());
                }
            }
            Client client = new Client(this,tcp, _Scripts);
            Clients_Add.Add(client);
        }
        catch { }
        AddCount--;
    }
    public void RemoveClient(Client _client)
    {
        RemoveCount++;
        try
        {
            Clients_Remove.Add(_client);
        }
        catch { }
        RemoveCount--;
    }
    public void Update()
    {
        if (!CanRun)
        {
            return;
        }
        CanRun = false;
        //===删除===
        if (RemoveCount == 0)
        {
            if (Clients_Remove.Count > 0)
            {
                for (int i = 0; i < Clients_Remove.Count; i++)
                {
                    if(Clients.Contains(Clients_Remove[i]))
                    {
                        Clients.Remove(Clients_Remove[i]);
                    }
                }
                Debug.Info("删除客户端");
                Clients_Remove.Clear();
            }
        }
        //===添加新的===
        if (AddCount == 0)
        {
            if (Clients_Add.Count > 0)
            {
                Clients.AddRange(Clients_Add);
                Clients_Add.Clear();
                Debug.Info("添加客户端");
            }
        }
        //===执行逻辑===
        for (int i = 0; i < Clients.Count; i++)
        {
            Clients[i].Update();
        }
        //====执行脚本
        for (int i = 0; i < Clients.Count; i++)
        {
            Clients[i].LateUpdate();
        }
        CanRun = true;
    }
    private bool CanRun = true;
}

