using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveTime 
{
    private static double m_time = 0f;

    private static double m_deltaTime = 0f;
    
    private static bool timePaused = false;
    public static float Time
    {
        get
        {
            return (float) m_time;
        }
    }

    public static float DeltaTime
    {
        get
        {
            return (float) m_deltaTime;
        }
    }

    public static void Update(double deltaTime)
    {
        if (timePaused) return;
        m_deltaTime = deltaTime;
        m_time += deltaTime;
    }

    public static void PauseTime()
    {
        m_deltaTime = 0;
        timePaused = true;
    }

    public static void ResumeTime()
    {
        timePaused = false;
    }
}
