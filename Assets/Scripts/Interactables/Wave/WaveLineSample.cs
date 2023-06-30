using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class WaveLineSample : MonoBehaviour {
        
        public void UpdateEField(Vector3 vec) {
            this.transform.LookAt((this.transform.position + this.transform.forward), vec);
            this.transform.localScale = new Vector3(1, vec.magnitude, 1);
        }
    }
}