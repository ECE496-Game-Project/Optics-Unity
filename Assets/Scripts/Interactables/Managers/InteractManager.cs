using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonUtils;
namespace GO_Wave {
    /// <summary>
    /// 与控制Wave与Device的交互，读取写入对象为WaveSource，Device
    /// </summary>
    public class InteractManager : MonoSingleton<InteractManager> {

        Dictionary<WaveSource, List<WaveSource>> WaveSourceList;

        #region PRIVATE METHOD
        // [TODO][WAVEGENERATE]: Generate Wave's Child Wave, base on devices
        public WaveSource GenerateChildWave(WaveSource srcWs, int parentId) {
            //GameObject go = new GameObject(srcWs.name + "_Child", typeof(WaveSource));
            //go.transform.parent = srcWs.transform;
            //WaveSource destWs = go.GetComponent<WaveSource>();

            return null;
        }
        #endregion

        void Start() {
            /*Find All WaveSource in Scene and put into WaveSourceList*/
            WaveSourceList = new Dictionary<WaveSource, List<WaveSource>>();
            foreach (WaveSource ws in Resources.FindObjectsOfTypeAll(typeof(WaveSource))) {
                if (EditorUtility.IsPersistent(ws.transform.root.gameObject) && !(ws.hideFlags == HideFlags.NotEditable || ws.hideFlags == HideFlags.HideAndDontSave)) {
                    WaveSourceList.Add(ws, null);
                }
            }

            // [TODO][WAVEGENERATE]: redesign, better use while loop with a temparay Queue to do WaveGenerating
            // since the Generated Child Wave might generate new Child Wave, we need to set a distance limitation
            // and also not use simple for loop to do this task. (Use WaveSource.EffectDistance)

            /* for (int i = 0; i < WaveSourceList.Count; i++) {
                switch (WaveSourceList[i].ws.ActiveWaveParams.Type) {
                    case WaveUtils.WAVETYPE.PARALLEL:
                        WaveSourceList.Add((i, GenerateParallelChildWave(WaveSourceList[i].ws, i)));
                        break;
                    case WaveUtils.WAVETYPE.POINT:
                        WaveSourceList.Add((i, GeneratePointChildWave(WaveSourceList[i].ws, i)));
                        break;
                    default:
                        break;
                }
            }*/

        }
    }
}