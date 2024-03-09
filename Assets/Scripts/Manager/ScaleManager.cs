using UnityEngine;
using WaveUtils;

[System.Serializable]
public class ScaleManager
{
    [SerializeField]
    private float m_nmPerUnit = 100f;

    public float nmPerUnit => m_nmPerUnit;

    [SerializeField]
    private float m_fsPerUnitySecond = 0.5f;

    public float fsPerUnitySecond => m_fsPerUnitySecond;
    //[SerializeField]
    //private float m_longestWaveLength = 700;

    //[SerializeField]
    //private float m_longestDistanceForOnePeriod = 10;

    //private float m_smallestWaveFrequency => WaveAlgorithm.C / m_longestWaveLength;

    //[SerializeField]
    //private float m_longestDurationForOnePeriod = 2f;

    //private float m_smallestFrequency => 1 / m_longestDurationForOnePeriod;

    //private float m_waveSpeed = WaveAlgorithm.C;


    //private float m_longestWavePeriod => m_longestWaveLength / 300;

    //private bool rescale = false;
    //public void RescaleWaveSpeed()
    //{
    //    if (rescale)
    //    {
    //        Debug.LogError("RescaleWaveSpeed is already called");
    //    }
    //    else
    //    {
    //        Debug.Log(m_smallestWaveFrequency);
    //        Debug.Log(m_smallestFrequency);
    //        WaveAlgorithm.C = WaveAlgorithm.C * (m_longestWaveLength / m_longestDistanceForOnePeriod) * (m_smallestFrequency / m_smallestWaveFrequency);
            
    //    }
        
    //}

    //public float distanceRealToUI(float distance)
    //{
    //    return distance / m_longestDistanceForOnePeriod * m_longestWaveLength;
    //}

    //public float distanceUIToReal(float distance)
    //{
    //    return distance / m_longestWaveLength * m_longestDistanceForOnePeriod;
    //}


    //public float timeRealToUI(float time)
    //{
    //    return time / m_longestWavePeriod * m_longestWaveLength;
    //}
}