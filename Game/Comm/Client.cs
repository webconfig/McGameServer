﻿using System;
using System.Net.Sockets;
using System.Collections.Generic;

public class Client
{
    public TcpClient _client;
    public NetworkStream _stream;
    public Dictionary<string,Script_Base> Scripts=new Dictionary<string, Script_Base>();
    public Dictionary<int, CallBack<int, Byte[]>> Actions = new Dictionary<int, CallBack<int, byte[]>>();
    //=================
    private List<byte> AllDatas;
    private byte[] recieveData;
    private Int32 ReceiveBufferSize = 5 * 1024;
    public System.DateTime StartTime;
    private int len = 0;
    private int command = 0;
    //=====事件====
    public event CallBack ConnEvent;
    public event CallBack<Client> DisConnEvent;
    public Client()
    {
        AllDatas = new List<byte>();
        recieveData = new byte[ReceiveBufferSize];
    }
    public Client(TcpClient client)
    {
        AllDatas = new List<byte>();
        recieveData = new byte[ReceiveBufferSize];
        Init(client);
    }
    public void Init(TcpClient client)
    {
        _client = client;
        _stream = client.GetStream();
        StartTime = System.DateTime.Now;
        BeginRead();
    }
    public void DisConn()
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
            DisConnEvent(this);
        }
        //====
        foreach (var item in Scripts)
        {
            item.Value.End();
        }
        Scripts.Clear();
        Actions.Clear();
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
            DisConn();
        }
    }
    private void OnReceiveCallback(IAsyncResult ar)
    {
        int length = 0;
        try
        {
            length = _stream.EndRead(ar);
        }
        catch
        {
            Debug.Error("接收数据错误，然后退出");
            DisConn();
            return;
        }
        if (length == 0)
        {
            Debug.Error("接收数据长度：" + length);
            DisConn();
            return;
        }
        else if (length > 0)
        {
            //拷贝到缓存队列
            for (int i = 0; i < length; i++)
            {
                AllDatas.Add(recieveData[i]);
            }
            //===解析数据===
            do
            {
                if (AllDatas.Count > 7)//最小的包应该有8个字节
                {
                    NetHelp.BytesToInt(AllDatas, 0, ref len);//读取消息体的长度
                    len += 4;
                    //读取消息体内容
                    if (len <= AllDatas.Count)
                    {
                        //Debug.Info("解析出数据包数据：" + PackNum);
                        NetHelp.BytesToInt(AllDatas, 4, ref command);//操作命令
                        byte[] msgBytes = new byte[len - 8];
                        AllDatas.CopyTo(8, msgBytes, 0, msgBytes.Length);
                        AllDatas.RemoveRange(0, len);
                        //Debug.Info("删除数据：" + len+",剩余数据："+ AllDatas.Count);
                        int command_script = command / 100;
                        int command_local = command % 100;
                        if (Actions.ContainsKey(command_script))
                        {
                            Actions[command_script](command_local, msgBytes);
                        }
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
            } while(true);
        }
        BeginRead();
    }
    public void CallConnEvent()
    {
        if(ConnEvent!=null)
        {
            ConnEvent();
        }
    }
}

