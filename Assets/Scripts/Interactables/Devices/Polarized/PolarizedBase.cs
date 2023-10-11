using UnityEngine;
using System.Collections.Generic;
using GO_Wave;
using WaveUtils;
using CommonUtils;

namespace GO_Device {

    public class PolarizedBase : DeviceBase {
        public float _thicknessOffset;

        public virtual ComplexMatrix2X2 JohnsMatrix { get; }

#if DEBUG_DEVICE
        [Header("DEBUG_DEVICE")]        
        [SerializeField] protected Dictionary<WaveSource, WaveSource> m_childParentPair;
#else
        protected Dictionary<WaveSource, WaveSource> m_childParentPair;
#endif

        public override void WaveHit(in RaycastHit hit, WaveSource parentWS) {
            if (parentWS.Params.Type != WAVETYPE.PARALLEL) {
                DebugLogger.Warning(this.name, "PolarizedDevice only support Parallel Wave! Will not Do anything.");
                return;
            }
            /*GO Setup*/
            GameObject new_GO = new GameObject(parentWS.name + "_Child", typeof(WaveSource), typeof(LineWaveRender), typeof(LineWaveLogic));
            new_GO.transform.position = hit.point + Vector3.Normalize(hit.point - parentWS.transform.position) * _thicknessOffset;
            new_GO.transform.rotation = parentWS.transform.rotation;

            /*Wave Source, Display, Interact Setup*/
            WaveSource childWS = new_GO.GetComponent<WaveSource>();
            LineWaveRender lwd = new_GO.GetComponent<LineWaveRender>();
            LineWaveLogic lwi = new_GO.GetComponent<LineWaveLogic>();

            WaveParams new_WSP = new WaveParams(parentWS.Params);

            ComplexVector2 resVec = new ComplexVector2();

            WaveAlgorithm.WaveToJohnsVector(parentWS.Params, resVec);
            resVec = JohnsMatrix * resVec;

            WaveAlgorithm.JohnsVectorToWave(resVec, new_WSP);

            float tmpDistance = parentWS.Params.EffectDistance;
            parentWS.Params.EffectDistance = hit.distance;
            new_WSP.EffectDistance = tmpDistance - hit.distance;

            childWS._awake(new_WSP);
            lwd.SyncRootParam(parentWS.WaveDisplay);
            lwi.SyncRootParam(parentWS.WaveInteract);

            /*Store Pair*/
            m_childParentPair.Add(parentWS, childWS);
        }

        public override void WaveClean(WaveSource parentWS) {
            m_childParentPair[parentWS].WaveInteract.CleanInteract();
            Destroy(m_childParentPair[parentWS].gameObject);
            m_childParentPair.Remove(parentWS);
        }

        public void Awake() {
            m_childParentPair = new Dictionary<WaveSource, WaveSource>();
        }
    }
}