using UnityEngine;
using System.Collections.Generic;
using GO_Wave;
using WaveUtils;
using Interfaces;
using ParameterTransfer;
using System;
namespace GO_Device {

    public class PolarizedBase : DeviceBase, I_ParameterTransfer {
        public float ThicknessOffset;
        private WaveSource m_parent;
        private WaveSource m_child;
        private RaycastHit m_hit;

        public virtual ComplexMatrix2X2 JohnsMatrix { get; }

        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) { }

        public void ParameterChangeTrigger() {
            m_parent.ParameterChangeTrigger();
        }

        public override void WaveHit(in RaycastHit hit, WaveSource parentWS) {
            m_hit = hit;

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

            Vector3 r = new_GO.transform.position - parentWS.transform.position;
            parPhi = WaveAlgorithm.CalculateTravelAccumulatedPhase(r, parK, parPhi, parentWS.Params.KHat);

            float tmpDistance = parentWS.EffectDistance;
            parentWS.EffectDistance = hit.distance;

            childWS._awake(
                new WaveParams(
                    parType, resEox, resEoy,
                    parW, parK, parN,
                    resTheta, parPhi,
                    tmpDistance - hit.distance
                )
            );
            lwd.SyncRootParam(parentWS.WaveDisplay);
            lwi.SyncRootParam(parentWS.WaveInteract);

            /*Store Pair*/
            m_parent = parentWS;
            m_child = childWS;
        }

        public override void CleanDeviceHitTrace(WaveSource parentWS) {
            if (m_parent == null && m_child == null) return;
            m_child.WaveClean();
            Destroy(m_child.gameObject);
            m_child = null;
            m_parent = null;
        }

        public void Awake() {
            m_child = null;
            m_parent = null;
        }
    }
}