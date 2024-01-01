using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

using CommonUtils;
using Constraint;

namespace Panel {
    public class DeviceOrderPanel : MonoSingleton<DeviceOrderPanel> {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _styleSheet;

        private VisualElement _root;
        private List<VisualElement> _devices_panel_list;

        [SerializeField] private int SlotNum = 8;

        private void OnEnable() {
            Generate();
        }

        void Generate() {
            if (_uiDocument == null || _styleSheet == null) return;

            _root = _uiDocument.rootVisualElement;
            _root.styleSheets.Add(_styleSheet);
            _root.AddToClassList("root");

            for (int i = 0; i < SlotNum; i++) {
                Image img = new Image();
                img.AddToClassList("slot");
                _root.Add(img);
            }

            //WaveOrderManager.Instance.DeviceCount;
        }

        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}