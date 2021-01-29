using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    static public bool paused;
    float beginPauseTime;
    float interval;

    void Start()
    {
        paused = false;
        interval = 0f;
    }

    void Update()
    {
        if (paused) return;

        if(Time.realtimeSinceStartup - beginPauseTime > interval)
        {
            Time.timeScale = 1;
        }
    }

    public void DoPause(float interval_in)
    {
        beginPauseTime = Time.realtimeSinceStartup;
        interval = interval_in;
        Time.timeScale = 0;
    }
}
