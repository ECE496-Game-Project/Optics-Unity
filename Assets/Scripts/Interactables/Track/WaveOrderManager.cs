

using Panel;
using GO_Device;
using GO_Wave;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using static Constraint.WaveDeviceOrder;
using CommonUtils;

namespace Constraint {
    public class WaveOrderManager : MonoSingleton<WaveOrderManager> {
        [SerializeField] private int MaxDeviceCount = 4;
        [SerializeField] private int DeviceSeperationDistance = 10;

        public int DeviceCount {
            get { return m_waveDeviceOrder.DeviceCount; }
        }

        WaveDeviceOrder m_waveDeviceOrder;

        private DeviceDragController m_deviceDragController;

        [SerializeField] private GameObject m_uiManager;


        private void Awake() {


            m_waveDeviceOrder = new WaveDeviceOrder(MaxDeviceCount, DeviceSeperationDistance);

            // assume the first child is my wave source
            Assert.IsTrue(transform.childCount > 0);
            var waveSource = transform.GetChild(0)?.GetComponent<WaveSource>();
            Assert.IsNotNull(waveSource);

            m_waveDeviceOrder.WaveSource = waveSource;

            // assume the rest of the children are my wave devices
            for (int i = 1; i < transform.childCount; i++) {
                var waveDevice = transform.GetChild(i)?.GetComponent<DeviceBase>();
                Assert.IsNotNull(waveDevice);

                m_waveDeviceOrder.AppendDevice(waveDevice);

                RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(i - 1));
            }

            SetDevicePositions();

            m_deviceDragController = GetComponent<DeviceDragController>();
            Assert.IsNotNull(m_deviceDragController);
            m_deviceDragController.Init(this, waveSource.transform.position, transform.position + new Vector3(0, 0, (MaxDeviceCount + 1) * DeviceSeperationDistance));
            WaitForOneFixedUpdateAndTrigger(waveSource, null);
        }

        private void SetDevicePositions() {
            for (int i = 0; i < m_waveDeviceOrder.DeviceCount; i++) {
                Transform deviceTransform = m_waveDeviceOrder.GetDevice(i).transform;

                deviceTransform.position = m_waveDeviceOrder.GetDevicePosition(i);
            }
        }

        public void SwapDeviceOrder(int firstDeviceIdx, int secondDeviceIdx) {
            Assert.IsFalse(firstDeviceIdx == secondDeviceIdx);

            int higherIdx = (firstDeviceIdx > secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;
            int lowerIdx = (firstDeviceIdx < secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;

            var higherIdxDevice = m_waveDeviceOrder.GetDeviceOrderInfo(higherIdx);

            var lowerIdxDevice = m_waveDeviceOrder.GetDeviceOrderInfo(lowerIdx);

            m_waveDeviceOrder.ReplaceDevice(higherIdxDevice, lowerIdx);
            m_waveDeviceOrder.ReplaceDevice(lowerIdxDevice, higherIdx);

            SetDevicePositions();

            //might also need to update gameobject hierarchy
            higherIdxDevice.device.transform.SetSiblingIndex(lowerIdx + 1);
            lowerIdxDevice.device.transform.SetSiblingIndex(higherIdx + 1);

            // the device which is currently in has the lower hierarchy was the device that has the higher hierarchy
            // we need to notify this device's parent wave source that need to regenerate

            var deviceParameterTransfer = (I_ParameterPanel)lowerIdxDevice;

            Assert.IsNotNull(deviceParameterTransfer);

            m_uiManager.SetActive(false);
            WaitForOneFixedUpdateAndTrigger(deviceParameterTransfer, () => { m_uiManager.SetActive(true); });


        }

        public void SwapDeviceOrderPlain(int firstDeviceIdx, int secondDeviceIdx) {
            Assert.IsFalse(firstDeviceIdx == secondDeviceIdx);

            int higherIdx = (firstDeviceIdx > secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;
            int lowerIdx = (firstDeviceIdx < secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;

            var higherIdxDevice = m_waveDeviceOrder.GetDeviceOrderInfo(higherIdx);

            var lowerIdxDevice = m_waveDeviceOrder.GetDeviceOrderInfo(lowerIdx);

            m_waveDeviceOrder.ReplaceDevice(higherIdxDevice, lowerIdx);
            m_waveDeviceOrder.ReplaceDevice(lowerIdxDevice, higherIdx);


            //might also need to update gameobject hierarchy
            higherIdxDevice.device.transform.SetSiblingIndex(lowerIdx + 1);
            lowerIdxDevice.device.transform.SetSiblingIndex(higherIdx + 1);


        }

        public void RemoveDevice(int deviceIdx) {
            var device = m_waveDeviceOrder.GetDevice(deviceIdx);
            var deviceInfo = m_waveDeviceOrder.GetDeviceOrderInfo(deviceIdx);
            m_waveDeviceOrder.removeDevice(deviceIdx);
            UnRegisterClickEvent(deviceInfo);

            // disable the collider so that the wave source can pass through
            device.gameObject.GetComponent<Collider>().enabled = false;
            DeviceOnUnClick(deviceIdx);

            SetDevicePositions();

            m_uiManager.SetActive(false);
            // Destroy the object after param is trigger
            WaitForOneFixedUpdateAndTrigger((I_ParameterPanel)device,
                () => {
                    Destroy(device.gameObject);
                    m_uiManager.SetActive(true);
                });
        }

        public void AddDevice(string deviceType) {

            if (m_waveDeviceOrder.DeviceCount == MaxDeviceCount) return;
            var newDevice = DevicePrefabLibrary.Instance.CreateDevice(deviceType);

            // when we add a new device we assume its rotation is its default rotation
            Quaternion newDeviceRotation = newDevice.transform.rotation;
            newDevice.transform.parent = transform;

            // in the wave track point of view, the new device is the default rotation
            newDevice.transform.localRotation = newDeviceRotation;

            if (m_waveDeviceOrder.DeviceCount == 0) {
                m_waveDeviceOrder.AppendDevice(newDevice);
                RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(m_waveDeviceOrder.DeviceCount - 1));
                SetDevicePositions();
                m_uiManager.SetActive(false);
                WaitForOneFixedUpdateAndTrigger((I_ParameterPanel)gameObject.transform.GetChild(0).GetComponent<WaveSource>(), () => { m_uiManager.SetActive(true); });

                return;
            }
            var lastSecondDevice = m_waveDeviceOrder.GetDevice(m_waveDeviceOrder.DeviceCount - 2);

            m_waveDeviceOrder.AppendDevice(newDevice);
            RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(m_waveDeviceOrder.DeviceCount - 1));
            SetDevicePositions();

            m_uiManager.SetActive(false);
            WaitForOneFixedUpdateAndTrigger((I_ParameterPanel)lastSecondDevice, () => { m_uiManager.SetActive(true); });
        }

        public void AddDevice(DEVICETYPE deviceType) {
            if (m_waveDeviceOrder.DeviceCount == MaxDeviceCount) return;
            var newDevice = DevicePrefabLibrary.Instance.CreateDevice(deviceType);

            // when we add a new device we assume its rotation is its default rotation
            Quaternion newDeviceRotation = newDevice.transform.rotation;
            newDevice.transform.parent = transform;

            // in the wave track point of view, the new device is the default rotation
            newDevice.transform.localRotation = newDeviceRotation;

            if (m_waveDeviceOrder.DeviceCount == 0) {
                m_waveDeviceOrder.AppendDevice(newDevice);
                RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(m_waveDeviceOrder.DeviceCount - 1));
                SetDevicePositions();
                m_uiManager.SetActive(false);
                WaitForOneFixedUpdateAndTrigger((I_ParameterPanel)gameObject.transform.GetChild(0).GetComponent<WaveSource>(), () => { m_uiManager.SetActive(true); });

                return;
            }

            m_waveDeviceOrder.AppendDevice(newDevice);
            var lastSecondDevice = m_waveDeviceOrder.GetDevice(m_waveDeviceOrder.DeviceCount - 2);


            m_uiManager.SetActive(false);
            RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(m_waveDeviceOrder.DeviceCount - 1));
            SetDevicePositions();
            WaitForOneFixedUpdateAndTrigger((I_ParameterPanel)lastSecondDevice, () => { m_uiManager.SetActive(true); });
        }


        void WaitForOneFixedUpdateAndTrigger(I_ParameterPanel i_ParameterTransfer, UnityAction extraBehavior) {
            StartCoroutine(WaitForOneFixedUpdateAndTriggerCoroutine(i_ParameterTransfer, extraBehavior));
        }

        IEnumerator WaitForOneFixedUpdateAndTriggerCoroutine(I_ParameterPanel i_ParameterTransfer, UnityAction extraBehavior) {
            yield return new WaitForFixedUpdate();

            i_ParameterTransfer.ParameterChangeTrigger();
            extraBehavior?.Invoke();
        }

        private Dictionary<int, UnityAction> deviceClickActions = new Dictionary<int, UnityAction>();
        private Dictionary<int, UnityAction> deviceUnclickAction = new Dictionary<int, UnityAction>();
        void RegisterClickEvent(DeviceOrderInfo deviceOrderInfo) {
            UnityAction clickAction = () => {
                DeviceOnClick(deviceOrderInfo.index);
            };

            UnityAction unclickAction = () => {
                DeviceOnUnClick(deviceOrderInfo.index);
            };

            //deviceOrderInfo.device.OnDeviceSelected.AddListener(clickAction);
            //deviceOrderInfo.device.OnDeviceUnselected.AddListener(unclickAction);
            deviceClickActions.Add(deviceOrderInfo.device.gameObject.GetInstanceID(), clickAction);
            deviceUnclickAction.Add(deviceOrderInfo.device.gameObject.GetInstanceID(), unclickAction);
        }

        void UnRegisterClickEvent(DeviceOrderInfo deviceOrderInfo) {
            Assert.IsNotNull(deviceOrderInfo.device);
            Assert.IsNotNull(deviceOrderInfo.device);
            int id = deviceOrderInfo.device.gameObject.GetInstanceID();

            Assert.IsTrue(deviceClickActions.ContainsKey(id));
            Assert.IsTrue(deviceUnclickAction.ContainsKey(id));


            //deviceOrderInfo.device.OnDeviceSelected.RemoveListener(deviceClickActions[id]);
            //deviceOrderInfo.device.OnDeviceUnselected.RemoveListener(deviceUnclickAction[id]);

            deviceClickActions.Remove(id);
            deviceUnclickAction.Remove(id);
        }


        private int m_selectedDeviceIdx = -1;
        void DeviceOnClick(int idx) {
            m_selectedDeviceIdx = idx;
            m_deviceDragController.SetTarget(m_waveDeviceOrder.GetDeviceOrderInfo(idx));

        }

        void DeviceOnUnClick(int idx) {
            m_selectedDeviceIdx = -1;
            m_deviceDragController.Clear();
        }

        public void removeSelectedDevice() {
            if (m_selectedDeviceIdx == -1) {
                Debug.Log("no device selected");
                return;
            }
            RemoveDevice(m_selectedDeviceIdx);

            return;
        }

        public int GetNewIdx(Vector3 newPosition, int originalIdx) {
            return m_waveDeviceOrder.GetDevicePositionIndex(newPosition, originalIdx);
        }

        public void ChangeIdx(int oldIdx, int newIdx) {
            if (oldIdx == newIdx) {
                SetDevicePositions();
                return;
            }

            if (oldIdx < newIdx) {
                for (int i = oldIdx; i < newIdx; i++) {
                    SwapDeviceOrderPlain(i, i + 1);



                }
                SetDevicePositions();
                var deviceParameterTransfer = (I_ParameterPanel)m_waveDeviceOrder.GetDevice(newIdx);
                Assert.IsNotNull(deviceParameterTransfer);

                m_uiManager.SetActive(false);

                WaitForOneFixedUpdateAndTrigger(deviceParameterTransfer, () => { m_uiManager.SetActive(true); });
            }
            else {
                for (int i = oldIdx; i > newIdx; i--) {
                    SwapDeviceOrderPlain(i, i - 1);
                }

                SetDevicePositions();
                var deviceParameterTransfer = (I_ParameterPanel)m_waveDeviceOrder.GetDevice(newIdx + 1);
                Assert.IsNotNull(deviceParameterTransfer);

                m_uiManager.SetActive(false);
                WaitForOneFixedUpdateAndTrigger(deviceParameterTransfer, () => { m_uiManager.SetActive(true); });
            }


        }
    }
}
