
using GO_Wave;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using WaveUtils;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace Inteference
{

    public class RegionManager:MonoBehaviour
    {
        public List<WaveInfo> m_waves = new List<WaveInfo>();

        [SerializeField]
        private List<WaveSource> m_sources;

        [SerializeField]
        private AABBRectangle m_region;

        [SerializeField]
        private float m_sampleResolution;

        [SerializeField]
        private GameObject m_intensityBallPrefab;

        [SerializeField]
        private Color m_zeroIntensityColor, m_maxIntensityColor;

        [SerializeField]
        private float m_highestIntensity = 1;
        public float HighestIntensity { get { return m_highestIntensity; } }

        private List<GameObject> m_ballList = new List<GameObject>();
        public void SpawnBalls()
        {
            for (float x = m_region.minCorner.x; x < m_region.maxCorner.x; x += m_sampleResolution)
            {
                for (float z = m_region.minCorner.z; z < m_region.maxCorner.z; z += m_sampleResolution)
                {
                    Vector3 position = new Vector3(x, m_region.minCorner.y, z);

                    GameObject ball = Instantiate(m_intensityBallPrefab, position, Quaternion.identity, this.transform);
                    ball.transform.localScale = ball.transform.localScale * m_sampleResolution;
                    m_ballList.Add(ball);

                }
            }
        }

        public void DeleteAllBall()
        {
            while(m_ballList.Count > 0)
            {
                GameObject ball = m_ballList[m_ballList.Count - 1];
                m_ballList.RemoveAt(m_ballList.Count - 1);
                Destroy(ball);
            }
        }

        private void Start()
        {
            SpawnBalls();
        }


        public void Render()
        {
            foreach (GameObject ball in m_ballList)
            {
                Renderer ballRenderer = ball.GetComponent<Renderer>();
                Vector3 E = Vector3.zero;

                foreach(WaveInfo waveSwitch in m_waves)
                {
                    if (waveSwitch.isOn)
                    {
                        Wave wave = waveSwitch.wave;
                        Vector3 vec = WaveAlgorithm.CalcIrradiance(
                            ball.transform.position,
                            WaveTime.Time,
                            wave.Params
                        );
                        E += vec;
                        
                        
                    }
                }
                float intensity = E.sqrMagnitude;
                ballRenderer.material.color = Color.Lerp(m_zeroIntensityColor, m_maxIntensityColor, intensity/HighestIntensity);
                
            }
        }
        private void Update()
        {
            if (m_waves.Count == 0)
            {
                for(int i = 0; i < m_sources.Count; i++)
                {
                    WaveSource source = m_sources[i];

                    foreach (Wave wave in source.generatedWaves)
                    {
                        m_waves.Add(new WaveInfo(wave));
                    }
                   
                }
                
            }
            Render();
        }


    }

    public class WaveInfo
    {
        public Wave wave;
        public string name => wave.gameObject.name;
        public bool isOn = true;
        public UnityEvent<bool> UIisCalcChangeCallRegionManager = new UnityEvent<bool>();

        public WaveInfo(Wave wave)
        {
            this.wave = wave;
            UIisCalcChangeCallRegionManager.AddListener((isOnFromUI) => { isOn = isOnFromUI; });
        }
    }


    [System.Serializable]
    public struct AABBRectangle
    {
        public Vector3 minCorner;
        public Vector3 maxCorner;

        public AABBRectangle(Vector3 min, Vector3 max)
        {
            minCorner = min;
            maxCorner = max;
        }
    }
}
