using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StopWatch
{
    public float[] usedTimes { get { return usedTimeList.ToArray(); } private set { } }
    public bool isPause { get; private set; }

    private List<float> usedTimeList;
    private float startTime;
    private float endTime;
    private float lastUsedTime;

    public StopWatch()
    {
        isPause = false;
        usedTimeList = new List<float>(5);
        Debug.Log("开始计时");
        startTime = Time.realtimeSinceStartup;
    }

    public void Restart()
    {
        if (isPause)
        {
            Debug.Log("继续计时");
            startTime = Time.realtimeSinceStartup;
            isPause = !isPause;
        }
        else
        {
            Debug.Log("计时中...");
        }
    }

    public void Pause()
    {
        if (!isPause)
        {
            lastUsedTime = Time.realtimeSinceStartup - startTime;
            usedTimeList.Add(lastUsedTime);
            isPause = !isPause;
            Debug.LogFormat("暂停计时, 上一段用时{0}(ms)", lastUsedTime * 1000);
        }
        else
        {
            Debug.LogFormat("已暂停计时, 上一段用时{0}(ms)", lastUsedTime * 1000);
        }
    }

    public void Stop()
    {
        if (!isPause)
        {
            endTime = Time.realtimeSinceStartup;
            lastUsedTime = endTime - startTime;
            usedTimeList.Add(lastUsedTime);
            isPause = !isPause;
        }
        Debug.LogFormat("计时结束, 共用时{0}(ms)", Amount() * 1000);
    }

    private float Amount()
    {
        float amount = 0;
        for(int i = 0;i< usedTimeList.Count; i++)
        {
            amount += usedTimeList[i];
        }
        return amount;
    }
}

