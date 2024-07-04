using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerData
{
    public string name;
    public int time;
    public int colorIndex;
    public TimerData(string name, int time, int colorIndex)
    {
        this.name = name;
        this.time = time;
        this.colorIndex = colorIndex;
    }
}   

[System.Serializable]
public class TimerSequence
{
    public string sequenceName;
    public List<TimerData> timers = new List<TimerData>();

    public TimerSequence(string sequenceName, List<TimerData> timers)
    {
        this.sequenceName = sequenceName;
        this.timers = timers;
    }

    public void SetName(string sequenceName)
    {
        this.sequenceName = sequenceName;
    }

    public void AddTimers(List<TimerData> nameTimePairs)
    {
        timers.Clear();
        for (int i = 0; i < nameTimePairs.Count; i++)
        {
            timers.Add(new TimerData(nameTimePairs[i].name, nameTimePairs[i].time, nameTimePairs[i].colorIndex));
        }
    }

    public int TotalTime()
    {
        int duration = 0;
        for (int i = 0; i < timers.Count; i++)
        {
            duration += timers[i].time;
        }
        return duration;
    }
}

[System.Serializable]
public class SaveData
{
    public List<TimerSequence> sequences = new List<TimerSequence>();
}

