using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Events;

using CommonUtils;
using Constraint;

namespace Panel {

    public static class VEHelper {
        public static VisualElement GenVE(params string[] classNames) {
            return GenVE<VisualElement>(classNames);
        }

        public static T GenVE<T>(params string[] classNames) where T : VisualElement, new() {
            var ele = new T();
            foreach (var className in classNames) {
                ele.AddToClassList(className);
            }

            return ele;
        }
    }

    public class Slot{
        Button deleteButton;
        Slider precSlider;
        FloatField floatInput;
        public VisualElement slot;

        public Slot(float initLoc, float slotMin, float slotMax, UnityAction<float> ValueChangeAct) {
            precSlider = VEHelper.GenVE<Slider>();
            precSlider.value = initLoc;
            precSlider.lowValue = slotMin;
            precSlider.highValue = slotMax;

            deleteButton = VEHelper.GenVE<Button>();
            deleteButton.text = "x";

            floatInput = VEHelper.GenVE<FloatField>();
            floatInput.value = precSlider.value;

            precSlider.RegisterValueChangedCallback(evt =>
            {
                floatInput.value = evt.newValue;
                ValueChangeAct?.Invoke(evt.newValue);
            });

            floatInput.RegisterValueChangedCallback(evt =>
            {
                precSlider.value = evt.newValue;
                ValueChangeAct?.Invoke(evt.newValue);
            });

            slot = VEHelper.GenVE("slot");
            slot.Add(precSlider);
            slot.Add(deleteButton);
            slot.Add(floatInput);
        }
    }
    
    public class DeviceOrderPanel : MonoSingleton<DeviceOrderPanel>{
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _styleSheet;

        private VisualElement _root;
        private VisualElement _panel;

        /* Slot informations */
        List<Slot> _slotsList;

        private void Awake() {
        }

        private void OnEnable() {
            Generate();
        }

        void Generate() {

            if (_uiDocument == null || _styleSheet == null) return;
            _root = _uiDocument.rootVisualElement;

            _root.styleSheets.Add(_styleSheet);
            _root.AddToClassList("root");

            _panel = VEHelper.GenVE("panel", "container");
            _root.Add(_panel);

            // Testing Propose
            InitSlots(new List<float> {0f, 0f, 0f, 0f});

            // UI Add Device 
            //Button addSlotButton = VEHelper.GenVE<Button>();
            //addSlotButton.text = "+";
            //_panel.Add(addSlotButton);
            // Call Add Deivce Lower level(distance in float)
        }


        private void AddDeivceButtonPress() {
            // WaveDeviceOrder.AddDevice(); return class contains,
            // {minValue, maxValue, valueChangeRC, deviceRemoveRC}
            // 1. track minValue maxValue, default Deivce position at maxValue
            // 2. RegisterCallback function(float) on valueChange
            // 3. RegisterCallback function() on deviceRemove
        }

        // Devices already in Scene
        public List<Slot> InitSlots(List<float> positionList) {
            foreach (float prec in positionList) {
                Slot slt = GenSlot(prec);

                _slotsList.Add(slt);
                _panel.Add(slt.slot);
            }

            return _slotsList;
        }

        public Slot GenSlot(float prec) {
            Slot newslt = new Slot(prec, 0, 1, (x) => { Debug.Log(x); });
            _slotsList.Add(newslt);
            return newslt;
        }

        
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }
    }
}