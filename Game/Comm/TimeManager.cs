using System.Timers;
public class TimeManager
{
    public static TimeManager Instance = new TimeManager();
    public event CallBack Update;
    public TimeManager()
    {
        Timer time = new Timer(0.01f);
        time.Elapsed += time_Elapsed;
        time.AutoReset = true;
        time.Enabled = true;
    }

    void time_Elapsed(object sender, ElapsedEventArgs e)
    {
        if(Update != null)
        {
            Update();
        }
    }
}

