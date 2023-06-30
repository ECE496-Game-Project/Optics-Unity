using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class WaveLineSample : MonoBehaviour {
        
        [HideInInspector] public WaveParams EffectiveWaveParam;
        public del_floatGetter TimeScale;

        public void init(string name, WaveParams wp, del_floatGetter tsGetter) {
            this.name = name;
            EffectiveWaveParam = wp;
            TimeScale = tsGetter;
        }

        public void FixedUpdate() {
            if (EffectiveWaveParam == null) return;
            Vector3 vec = WaveAlgorithm.CalcIrradiance(this.transform.position - EffectiveWaveParam.Origin(), Time.time * TimeScale(), EffectiveWaveParam);

            this.transform.LookAt((this.transform.position + this.transform.forward), vec);
            this.transform.localScale = new Vector3(1, vec.magnitude, 1);
        }
    }
}