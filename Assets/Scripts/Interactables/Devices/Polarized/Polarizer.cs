using GO_Wave;
using UnityEngine;
using WaveUtils;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public class Polarizer : PolarizeDevice {
        [SerializeField] private float _rotDeg;
        [SerializeField] private float _thicknessOffset;

        public override ComplexMatrix2X2 JohnsMatrix {
            get {
                float rotRad = _rotDeg * Mathf.Deg2Rad;
                return new ComplexMatrix2X2(
                    Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * -Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
                );
            }
        }

        public override void WaveHit(in RaycastHit hit, WaveSource sourceWS) {
            Debug.Log("Polarized WaveHit");
            /*GO Setup*/
            GameObject new_GO = new GameObject(sourceWS.name + "_Child", typeof(WaveSource), typeof(LineWaveDisplay), typeof(LineWaveInteract));
            new_GO.transform.position = hit.point + Vector3.Normalize(hit.point - sourceWS.transform.position) * _thicknessOffset;
            new_GO.transform.rotation = sourceWS.transform.rotation;

            /*Wave Source, Display, Interact Setup*/
            WaveSource lws = new_GO.GetComponent<WaveSource>();
            LineWaveDisplay lwd = new_GO.GetComponent<LineWaveDisplay>();
            LineWaveInteract lwi = new_GO.GetComponent<LineWaveInteract>();

            WaveParams new_WSP = new WaveParams(sourceWS.Params);

            ComplexVector2 resVec = new ComplexVector2();
            WaveAlgorithm.WaveToJohnsVector(sourceWS.Params, resVec);
            resVec = JohnsMatrix * resVec;
            WaveAlgorithm.JohnsVectorToWave(resVec, new_WSP);

            float tmpDistance = sourceWS.Params.EffectDistance;
            sourceWS.Params.EffectDistance = hit.distance;
            new_WSP.EffectDistance = tmpDistance - hit.distance;

            lws.Prepare(new_WSP);
            lwd.Prepare(sourceWS.WaveDisplay);
            lwi.Prepare(sourceWS.WaveInteract);
        }
    }
}