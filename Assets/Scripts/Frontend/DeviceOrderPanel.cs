using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using CommonUtils;
using Constraint;

namespace Panel {
    public class DeviceOrderPanel : MonoSingleton<DeviceOrderPanel>{
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _styleSheet;

        private VisualElement _root;
        private List<Image> _img_list;

        [SerializeField] private int SlotNum = 8;

        bool isDragging = false;
        Vector2 startMousePosition;
        Vector2 startImagePosition;

        private void Awake() {
            _img_list = new List<Image>();
        }

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
                _img_list.Add(img);
            }

        }

        public void DraggableElement() {
            var image = _img_list[0];

            image.RegisterCallback<PointerDownEvent>(evt =>
            {
                isDragging = true;
                image.style.position = Position.Absolute;
                image.style.left = evt.position.x;
                image.style.top = evt.position.y;

            });

            image.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (isDragging) {
                    Debug.Log("Position: " + evt.position);
                    Debug.Log("LocalPosition: " + evt.localPosition);
                    Debug.Log("DeltaPosition: " + evt.deltaPosition);
                    Debug.Log("\n\n");
                    image.style.left = evt.position.x;
                    image.style.top = evt.position.y;
                }
            });

            image.RegisterCallback<PointerUpEvent>(evt =>
            {
                isDragging = false;
            });
        }

        private void BeginDrag(PointerDownEvent evt) {
            isDragging = true;
            startMousePosition = evt.localPosition;
            //startImagePosition = image.transform.position;
            evt.StopPropagation();
        }

        private void Dragging(PointerMoveEvent evt) {
            //evt.
            if (evt.pressedButtons == 1) // Left mouse button (change to evt.button if available)
            {
                Debug.Log("Dragging");
            }
        }

        private void EndDrag(PointerUpEvent evt) {
            // Handle end of drag here
            Debug.Log("EndDrag");
        }

        void Start() {
            DraggableElement();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}