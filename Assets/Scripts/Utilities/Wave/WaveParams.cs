using System;
using UnityEngine;
using UnityEngine.Events;
using CommonUtils;

namespace WaveUtils {
	public enum WAVETYPE {
		INVALID = 0,
		PARALLEL,
		POINT
    }

	[System.Serializable]
	public class WaveParams {
		#region GLOBAL VAR
		public WAVETYPE Type = WAVETYPE.INVALID;

        public del_Vec3ParamVec3Getter UHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter VHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter KHat = Del_Default.DefaultVec3ParamVec3Getter;

		[Header("Magnitude Settings")]
		public float _eox;
		public float _eoy;

		[Header("Frequency Settings")]
		public float _w;
		public float _k;
		public float _n;

		[Header("Angle Settings in degree")]
		[Range(0, 360)]
		public float _theta;
		[Range(0, 360)]
		public float _phi;

		[Header("Dispersion Distance")]
		[SerializeField] private float _effectDistance = 100.0f;
		#endregion

		#region GLOBAL METHOD
		public float Eox {
			get { return _eox; }
			set {
				_eox = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float Eoy {
			get { return _eoy; }
			set {
				_eoy = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float W {
			get { return _w; }
			set {
				_w = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float K {
			get { return _k; }
			set {
				_k = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float N {
			get { return _n; }
			set {
				_n = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float Theta {
			get { return _theta; }
			set {
				_theta = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float Phi {
			get { return _phi; }
			set {
				_phi = value;
				if (!ListenerLock) NondestructableListener?.Invoke();
			}
		}
		public float EffectDistance {
			get { return _effectDistance; }
            set { 
				_effectDistance = value;
				if(!ListenerLock) DestructableListener?.Invoke();
			}
        }
        #endregion

        #region CALLBACK
        /// <summary>
        /// Set To True when don't want Listener to Callback.
        /// </summary>
        [HideInInspector] public bool ListenerLock = false;
		[HideInInspector] public UnityEvent NondestructableListener;
		[HideInInspector] public UnityEvent DestructableListener;
		#endregion

		#region CONSTRUCTOR
		public WaveParams() {
			NondestructableListener = new UnityEvent();
			DestructableListener = new UnityEvent();
		}
		public WaveParams(WaveParams src) {
			this.Type = src.Type;
			this.UHat = src.UHat;
			this.VHat = src.VHat;
            this.KHat = src.KHat;
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.W = src.W;
			this.K = src.K;
			this.N = src.N;
			this.Theta = src.Theta;
			this.Phi = src.Phi;

			NondestructableListener = new UnityEvent();
			DestructableListener = new UnityEvent();
		}
        #endregion
	}
}