using System.Collections.Generic;
using CommonUtils;
using GO_Wave;
using UnityEngine;
using WaveUtils;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public class Polarizer : PolarizeDevice {
        [SerializeField] private float _rotDeg;
        [SerializeField] private float _thicknessOffset;

        private Dictionary<WaveSource, WaveSource> _childParentPair;

        public override ComplexMatrix2X2 JohnsMatrix {
            get {
                float rotRad = _rotDeg * Mathf.Deg2Rad;
                return new ComplexMatrix2X2(
                    Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
                );
            }
        }

        public override void WaveHit(in RaycastHit hit, WaveSource parentWS) {
            if (parentWS.Params.Type != WAVETYPE.PARALLEL) {
                DebugLogger.Warning(this.name, "PolarizedDevice only support Parallel Wave! Will not Do anything.");
                return;
            }
            /*GO Setup*/
            GameObject new_GO = new GameObject(parentWS.name + "_Child", typeof(SubWaveSource), typeof(LineWaveDisplay), typeof(LineWaveInteract));
            new_GO.transform.position = hit.point + Vector3.Normalize(hit.point - parentWS.transform.position) * _thicknessOffset;
            new_GO.transform.rotation = parentWS.transform.rotation;

            /*Wave Source, Display, Interact Setup*/
            SubWaveSource childWS = new_GO.GetComponent<SubWaveSource>();
            LineWaveDisplay lwd = new_GO.GetComponent<LineWaveDisplay>();
            LineWaveInteract lwi = new_GO.GetComponent<LineWaveInteract>();

            WaveParams new_WSP = new WaveParams(parentWS.Params);

            ComplexVector2 resVec = new ComplexVector2();
            WaveAlgorithm.WaveToJohnsVector(parentWS.Params, resVec);
            resVec = JohnsMatrix * resVec;
            WaveAlgorithm.JohnsVectorToWave(resVec, new_WSP);

            float tmpDistance = parentWS.Params.EffectDistance;
            parentWS.Params.EffectDistance = hit.distance;
            new_WSP.EffectDistance = tmpDistance - hit.distance;

            childWS.Prepare(new_WSP);
            lwd.Prepare(parentWS.WaveDisplay);
            lwi.Prepare(parentWS.WaveInteract);

            /*Store Pair*/
            _childParentPair.Add(parentWS, childWS);
        }
    
        public override void WaveCleanup(WaveSource parentWS) {
            Destroy(_childParentPair[parentWS].gameObject);
            _childParentPair.Remove(parentWS);
        }

        public void Awake() {
            _childParentPair = new Dictionary<WaveSource, WaveSource>();
        }
    }
}