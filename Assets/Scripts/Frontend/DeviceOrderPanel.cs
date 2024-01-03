using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using CommonUtils;
using Constraint;

namespace Panel {
    public class DeviceLabel : VisualElement {
        Image DeviceIcon;

        DeviceLabel(Texture img) {
            DeviceIcon = new Image();
            DeviceIcon.image = img;
        }
    }

    public class DeviceOrderPanel : MonoSingleton<DeviceOrderPanel>{
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _styleSheet;

        private VisualElement _root;
        private VisualElement _bar;
        private List<Image> _img_list;

        /* Slot informations */
        [SerializeField] private int MaxSlot = 1;
        [SerializeField] private float Left = 20;
        [SerializeField] private float Width = 60;
        [SerializeField] private float Top = 90;

        bool isDragging = false;
        Image DraggingTarget;

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

            _bar = new VisualElement();
            _bar.AddToClassList("progress-container");
            _bar.style.left = new StyleLength(new Length(Left, LengthUnit.Percent));
            _bar.style.width = new StyleLength(new Length(Width, LengthUnit.Percent));
            _bar.style.top = new StyleLength(new Length(Top, LengthUnit.Percent));

            _root.Add(_bar);

            for (int i = 0; i < MaxSlot; i++) {
                Image img = GenIcon();
                
                _root.Add(img);
                _img_list.Add(img);
            }
        }

        public Image GenIcon() {
            Image img = new Image();
            img.image = Resources.Load<Texture2D>("Art/Images/Polarizer");
            img.AddToClassList("slot");
            img.style.right = new StyleLength(new Length((100 - Left - Width), LengthUnit.Percent));
            img.style.top = new StyleLength(new Length(Top, LengthUnit.Percent));

            img.RegisterCallback<PointerDownEvent>(evt => {
                isDragging = true;
                DraggingTarget = (Image)evt.currentTarget;
            });

            img.RegisterCallback<PointerUpEvent>(evt => {
                isDragging = false;
                DraggingTarget = null;
            });
            return img;
        }

        void Start() {
        }

        // Update is called once per frame
        void Update() {
            if (isDragging) {
                // Calculate the percentage of the mouse position relative to the screen width
                float xPrec = (Screen.width - Input.mousePosition.x / Screen.width) * 100;

                // Set the 'right' style property
                DraggingTarget.style.right = new StyleLength(new Length(xPrec, LengthUnit.Percent));
            }


        }
    }
}