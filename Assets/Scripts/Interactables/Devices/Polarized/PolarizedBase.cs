using UnityEngine;
using System.Collections.Generic;
using GO_Wave;
using WaveUtils;
using CommonUtils;
using Interfaces;
using System;

namespace GO_Device {

    public class PolarizedBase : DeviceBase/*, I_ParameterTransfer*/ {
        public float ThicknessOffset;
        
        public virtual ComplexMatrix2X2 JohnsMatrix { get; }

#if DEBUG_DEVICE
        [Header("DEBUG_DEVICE")]        
        [SerializeField] protected Dictionary<WaveSource, WaveSource> m_childParentPair;
#else
        protected Dictionary<WaveSource, WaveSource> m_childParentPair;
#endif
        public virtual bool ParameterSet<T>(string paramName, T value) {
            if (paramName == "ThicknessOffset") {
                ThicknessOffset = (float)Convert.ToDouble(value);
                return true;
            }
            return false;
        }
        public virtual T ParameterGet<T>(string paramName) {
            if (paramName == "ThicknessOffset") {
                return (T)(object)ThicknessOffset;
            }
            return default(T);
        }
        public override void WaveHit(in RaycastHit hit, WaveSource parentWS) {
            /*GO Setup*/
            GameObject new_GO = new GameObject(parentWS.name + "_Child", typeof(WaveSource), typeof(LineWaveRender), typeof(LineWaveLogic));
            new_GO.transform.position = hit.point + Vector3.Normalize(hit.point - parentWS.transform.position) * ThicknessOffset;
            new_GO.transform.rotation = parentWS.transform.rotation;

            /*Wave Source, Display, Interact Setup*/
            WaveSource childWS = new_GO.GetComponent<WaveSource>();
            LineWaveRender lwd = new_GO.GetComponent<LineWaveRender>();
            LineWaveLogic lwi = new_GO.GetComponent<LineWaveLogic>();

            ComplexVector2 resVec;
            WAVETYPE parType;
            float parEox, parEoy, parW, parK, parN, parTheta, parPhi;
            float resEox, resEoy, resTheta;
            parentWS.WaveParameterGetAll(out parType, out parEox, out parEoy, out parW, out parK, out parN, out parTheta, out parPhi);

            WaveAlgorithm.WaveToJohnsVector(
                parEox,
                parEoy,
                parTheta, 
                out resVec
            );

            resVec = JohnsMatrix * resVec;

            WaveAlgorithm.JohnsVectorToWave(
                resVec,
                out resEox,
                out resEoy,
                out resTheta
                );

            float tmpDistance = parentWS.EffectDistance;
            parentWS.EffectDistance = hit.distance;
            childWS.EffectDistance = tmpDistance - hit.distance;

            childWS._awake(
                new WaveParams(
                    parType, resEox, resEoy,
                    parW, parK, parN,
                    resTheta, parPhi
                )
            );
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