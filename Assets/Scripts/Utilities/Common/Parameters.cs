using UnityEngine;
using UnityEngine.Events;

namespace CommonUtils {
    [System.Serializable]
    public class Parameter<T> {
        [SerializeField] private T _value;

        [HideInInspector] public UnityEvent VTOMListener;
        [HideInInspector] public UnityEvent MTOVListener;

        public T Value {
            get { return _value; }
            set { _value = value; }
        }
        public T VTOMValue {
            set {
                _value = value;
                VTOMListener?.Invoke();
            }
        }
        public T MTODValue {
            set {
                _value = value;
                MTOVListener?.Invoke();
            }
        }

        public Parameter(T initalValue, UnityEvent DTOM, UnityEvent MTOD){
            _value = initalValue;
            VTOMListener = DTOM;
            MTOVListener = MTOD;
        }
    }
}