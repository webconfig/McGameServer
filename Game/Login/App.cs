using google.protobuf;
public class App
{
    public static ClientWorlds ClientDatas;
    public static Worlds Datas;
    public static void SetDatas(Worlds _Datas)
    {
        Datas = _Datas;
        ClientDatas = new ClientWorlds();
        for (int i = 0; i < _Datas.value.Count; i++)
        {
            ClientWorld cw = new ClientWorld();
            cw.ID = _Datas.value[i].ID;
            cw.Name = _Datas.value[i].Name;
        }
    }
}

