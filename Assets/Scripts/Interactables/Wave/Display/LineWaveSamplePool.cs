using System.Collections;
using UnityEngine;
using CommonUtils;
using UnityEngine.Pool;
using GO_Wave;
namespace ObjectPool {
    public class LineWaveSamplePool : NoDestroyMonoSingleton<LineWaveSamplePool> {
        #region INSPECTOR SETTINGS
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 1000;
        [SerializeField] private GameObject _samplePointPrefab;
        #endregion

        IObjectPool<LineWaveSample> m_Pool;

        public IObjectPool<LineWaveSample> Pool {
            get {
                if (m_Pool == null) 
                    m_Pool = new ObjectPool<LineWaveSample>(
                        CreatePooledItem, 
                        OnTakeFromPool, 
                        OnReturnedToPool, 
                        OnDestroyPoolObject, 
                        collectionChecks, 
                        maxPoolSize
                    );
                return m_Pool;
            }
        }

        LineWaveSample CreatePooledItem() {
            return Instantiate(_samplePointPrefab).GetComponent<LineWaveSample>();
        }

        void OnReturnedToPool(LineWaveSample sample) {
            sample.gameObject.SetActive(false);
        }

        void OnTakeFromPool(LineWaveSample sample) {
            sample.gameObject.SetActive(true);
        }

        void OnDestroyPoolObject(LineWaveSample sample) {
            Destroy(sample.gameObject);
        }
    }
}