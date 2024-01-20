//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Assertions;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;

//namespace Constraint.UI
//{
//    public class DeviceOrderUIManager : MonoBehaviour
//    {
//        private Button m_removeButton;
//        private Button m_addPolarizerButton;
//        private Button m_addQWPButton;
//        private Button m_addHWPButton;

//        private WaveOrderManager m_waveOrderManager;


//        private PlayerInput m_playerInput;

//        private List<RectTransform> m_corners = new List<RectTransform>();


//        private MouseSelect m_mouseSelect;

//        private bool m_isMouseInside = false;
//        // Start is called before the first frame update
//        void Awake()
//        {
//            m_removeButton = transform.Find("RemoveDeviceBtn").GetComponent<Button>();
//            m_addPolarizerButton = transform.Find("AddPolarizerBtn").GetComponent<Button>();
//            m_addQWPButton = transform.Find("AddQWPBtn").GetComponent<Button>();
//            m_addHWPButton = transform.Find("AddHWPBtn").GetComponent<Button>();

//            Assert.IsNotNull(m_removeButton);
//            Assert.IsNotNull(m_addPolarizerButton);
//            Assert.IsNotNull(m_addQWPButton);
//            Assert.IsNotNull(m_addHWPButton);




//            m_waveOrderManager = GameObject.Find("WaveTrack").GetComponent<WaveOrderManager>();

//            Assert.IsNotNull(m_waveOrderManager);

//            m_playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
//            Assert.IsNotNull(m_playerInput);

//            var topLeft = GameObject.Find("TopLeft").GetComponent<RectTransform>();
//            var topRight = GameObject.Find("TopRight").GetComponent<RectTransform>();
//            var bottomLeft = GameObject.Find("BottomLeft").GetComponent<RectTransform>();
//            var bottomRight = GameObject.Find("BottomRight").GetComponent<RectTransform>();
//            Assert.IsNotNull(topLeft);
//            Assert.IsNotNull(topRight);
//            Assert.IsNotNull(bottomLeft);
//            Assert.IsNotNull(bottomRight);

//            m_corners.Add(topLeft);
//            m_corners.Add(topRight);
//            m_corners.Add(bottomLeft);
//            m_corners.Add(bottomRight);


//        }

//        private void Start()
//        {
//            m_selectionController = TempSingletonManager.Instance.m_selectionController;
//            Assert.IsNotNull(m_selectionController);
//        }

//        private void OnEnable()
//        {
//            m_removeButton.onClick.AddListener(m_waveOrderManager.removeSelectedDevice);
//            m_addPolarizerButton.onClick.AddListener(() => { m_waveOrderManager.AddDevice(GO_Device.DEVICETYPE.POLARIZER); });
//            m_addQWPButton.onClick.AddListener(() => { m_waveOrderManager.AddDevice(GO_Device.DEVICETYPE.QUATERWAVEPLATE); });
//            m_addHWPButton.onClick.AddListener(() => { m_waveOrderManager.AddDevice(GO_Device.DEVICETYPE.HALFWAVEPLATE); });

//            m_playerInput.actions["MouseMovement"].performed += OnMouseMove;
//        }

//        private void OnDisable()
//        {

//            m_removeButton?.onClick.RemoveAllListeners();
//            m_addPolarizerButton?.onClick.RemoveAllListeners();
//            m_addQWPButton?.onClick.RemoveAllListeners();
//            m_addHWPButton?.onClick.RemoveAllListeners();

//            if (m_playerInput != null)
//            {
//                m_playerInput.actions["MouseMovement"].performed -= OnMouseMove;
//            }

//        }


//        public void OnMouseMove(InputAction.CallbackContext context)
//        {
//            Vector3 mousePosition = context.ReadValue<Vector2>();

//            if (!m_isMouseInside && isInside(mousePosition))
//            {
//                m_isMouseInside = true;
//                m_selectionController.TurnOff();
//            }
//            else if (m_isMouseInside && !isInside(mousePosition))
//            {
//                m_isMouseInside = false;
//                m_selectionController.TurnOn();
//            }

//        }

//        bool isInside(Vector3 point)
//        {
//            Vector3[] corners = new Vector3[4];
//            for (int i = 0; i < 4; i++)
//            {
//                corners[i] = m_corners[i].position;
//            }

//            if (corners[0].x <= point.x && point.x <= corners[1].x &&
//                corners[3].y <= point.y && point.y <= corners[0].y)
//            {
//                return true;
//            }
//            else {
//                return false;
//            }


//        }
//    }
//}
