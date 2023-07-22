using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonUtils;
using GO_Device;

namespace GO_Wave {
    /// <summary>
    /// 与控制Wave与Device的交互，读取写入对象为WaveSource，Device
    /// </summary>
    public class InteractManager : MonoSingleton<InteractManager> {

        Dictionary<WaveSource, List<WaveSource>> WaveSourceList;

        private LayerMask _interactMask;

        #region PRIVATE METHOD
        private void GenerateChildWave(WaveSource srcWs) {

            switch (srcWs.ActiveWaveParams.Type) {
                case WaveUtils.WAVETYPE.PARALLEL:
                    break;
                //[TODO][PointWave]:Generate Point Source ChildWaves
                case WaveUtils.WAVETYPE.POINT:
                    break;
                default:
                    break;
            }
        }

        private void WaveInteractDevice(WaveSource ws, DeviceBase db) {
            switch (db.type) {
                case DEVICETYPE.WEAVEPLATE:
                case DEVICETYPE.POLARIZER:
                    break;
                default:
                    break;
            }
        }

        private void GenerateParallelSrcChildWave(WaveSource srcWs) {
            /* Current Parallel WaveSrc to Device Detection only designed to shoot ray
             * from ORIGIN with direction FRONT, with Effective Distance.
             */
            RaycastHit hit;
            if(Physics.Raycast(srcWs.transform.position, srcWs.transform.forward, out hit, srcWs.EffectDistance, _interactMask)) {

            }

            //GameObject go = new GameObject(srcWs.name + "_Child", typeof(WaveSource));
            //go.transform.parent = srcWs.transform;
            //WaveSource destWs = go.GetComponent<WaveSource>();
        }
        #endregion

        void Start() {
            /*Find All WaveSource in Scene and put into WaveSourceList*/
            WaveSourceList = new Dictionary<WaveSource, List<WaveSource>>();
            foreach (WaveSource ws in Resources.FindObjectsOfTypeAll(typeof(WaveSource))) {
                if (EditorUtility.IsPersistent(ws.transform.root.gameObject) && !(ws.hideFlags == HideFlags.NotEditable || ws.hideFlags == HideFlags.HideAndDontSave)) {
                 
                    
                    /* Generate ChildWaveSources */

                    
                    
                    
                    
                    WaveSourceList.Add(ws, null);
                }
            }
        }
    }
}