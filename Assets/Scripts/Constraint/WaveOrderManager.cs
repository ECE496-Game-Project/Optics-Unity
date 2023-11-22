

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

namespace Constraint
{
    public class WaveOrderManager : MonoBehaviour
    {
        [SerializeField] private int MaxDeviceCount = 4;
        [SerializeField] private int DeviceSeperationDistance = 10;

        WaveDeviceOrder m_waveDeviceOrder;

        private void Awake()
        {
            m_waveDeviceOrder = new WaveDeviceOrder(MaxDeviceCount, DeviceSeperationDistance);

            // assume the first child is my wave source
            Assert.IsTrue(transform.childCount > 0);
            var waveSource = transform.GetChild(0)?.GetComponent<WaveSource>();
            Assert.IsNotNull(waveSource);

            m_waveDeviceOrder.WaveSource = waveSource;

            // assume the rest of the children are my wave devices
            for (int i = 1; i < transform.childCount; i++)
            {
                var waveDevice = transform.GetChild(i)?.GetComponent<DeviceBase>();
                Assert.IsNotNull(waveDevice);

                m_waveDeviceOrder.AppendDevice(waveDevice);

                RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(i - 1));
            }

            SetDevicePositions();

            WaitForOneFixedUpdateAndTrigger(waveSource, null);
        }

        private void SetDevicePositions()
        {
            for (int i = 0; i < m_waveDeviceOrder.DeviceCount; i++)
            {
                Transform deviceTransform = m_waveDeviceOrder.GetDevice(i).transform;

                deviceTransform.position = m_waveDeviceOrder.GetDevicePosition(i);
            }
        }

        public void SwapDeviceOrder(int firstDeviceIdx, int secondDeviceIdx)
        {
            Assert.IsFalse(firstDeviceIdx == secondDeviceIdx);

            int higherIdx = (firstDeviceIdx > secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;
            int lowerIdx = (firstDeviceIdx < secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;

            var higherIdxDevice = m_waveDeviceOrder.GetDevice(higherIdx);

            var lowerIdxDevice = m_waveDeviceOrder.GetDevice(lowerIdx);

            m_waveDeviceOrder.ReplaceDevice(higherIdxDevice, lowerIdx);
            m_waveDeviceOrder.ReplaceDevice(lowerIdxDevice, higherIdx);

            SetDevicePositions();

            //might also need to update gameobject hierarchy
            higherIdxDevice.transform.SetSiblingIndex(lowerIdx);
            lowerIdxDevice.transform.SetSiblingIndex(higherIdx);

            // the device which is currently in has the lower hierarchy was the device that has the higher hierarchy
            // we need to notify this device's parent wave source that need to regenerate

            var deviceParameterTransfer = (I_ParameterTransfer)lowerIdxDevice;

            Assert.IsNotNull(deviceParameterTransfer);

            WaitForOneFixedUpdateAndTrigger(deviceParameterTransfer, null);


        }

        public void RemoveDevice(int deviceIdx)
        {
            var device = m_waveDeviceOrder.GetDevice(deviceIdx);
            var deviceInfo = m_waveDeviceOrder.GetDeviceOrderInfo(deviceIdx);
            m_waveDeviceOrder.removeDevice(deviceIdx);
            UnRegisterClickEvent(deviceInfo);

            // disable the collider so that the wave source can pass through
            device.gameObject.GetComponent<Collider>().enabled = false;



            SetDevicePositions();

           
            // Destroy the object after param is trigger
            WaitForOneFixedUpdateAndTrigger((I_ParameterTransfer) device, 
                () => { Destroy(device.gameObject); });
        }

        public void AddDevice(string deviceType)
        {
            var newDevice = DevicePrefabLibrary.Instance.CreateDevice(deviceType);

            // when we add a new device we assume its rotation is its default rotation
            Quaternion newDeviceRotation = newDevice.transform.rotation;
            newDevice.transform.parent = transform;

            // in the wave track point of view, the new device is the default rotation
            newDevice.transform.localRotation = newDeviceRotation;

            var lastSecondDevice = m_waveDeviceOrder.GetDevice(m_waveDeviceOrder.DeviceCount - 2);

            m_waveDeviceOrder.AppendDevice(newDevice);
            RegisterClickEvent(m_waveDeviceOrder.GetDeviceOrderInfo(m_waveDeviceOrder.DeviceCount - 1));
            SetDevicePositions();
            WaitForOneFixedUpdateAndTrigger((I_ParameterTransfer)lastSecondDevice, null);
        }

        public void AddDevice(DEVICETYPE deviceType)
        {
            var newDevice = DevicePrefabLibrary.Instance.CreateDevice(deviceType);

            // when we add a new device we assume its rotation is its default rotation
            Quaternion newDeviceRotation = newDevice.transform.rotation;
            newDevice.transform.parent = transform;

            // in the wave track point of view, the new device is the default rotation
            newDevice.transform.localRotation = newDeviceRotation;

            var lastSecondDevice = m_waveDeviceOrder.GetDevice(m_waveDeviceOrder.DeviceCount - 2);

            m_waveDeviceOrder.AppendDevice(newDevice);
            SetDevicePositions();
            WaitForOneFixedUpdateAndTrigger((I_ParameterTransfer)lastSecondDevice, null);
        }


        void WaitForOneFixedUpdateAndTrigger(I_ParameterTransfer i_ParameterTransfer, UnityAction extraBehavior)
        {
            StartCoroutine(WaitForOneFixedUpdateAndTriggerCoroutine(i_ParameterTransfer, extraBehavior));
        }

        IEnumerator WaitForOneFixedUpdateAndTriggerCoroutine(I_ParameterTransfer i_ParameterTransfer, UnityAction extraBehavior)
        {
            yield return new WaitForFixedUpdate();

            i_ParameterTransfer.ParameterChangeTrigger();
            extraBehavior?.Invoke();
        }

        private Dictionary<int, UnityAction> deviceClickActions = new Dictionary<int, UnityAction>();
        private Dictionary<int, UnityAction> deviceUnclickAction = new Dictionary<int, UnityAction>();
        void RegisterClickEvent(DeviceOrderInfo deviceOrderInfo)
        {
            UnityAction clickAction = () =>
            {
                DeviceOnClick(deviceOrderInfo.index);
            };

            UnityAction unclickAction = () =>
            {
                DeviceOnUnClick(deviceOrderInfo.index);
            };

            deviceOrderInfo.device.OnDeviceSelected.AddListener(clickAction);
            deviceOrderInfo.device.OnDeviceUnselected.AddListener(unclickAction);
            deviceClickActions.Add(deviceOrderInfo.device.gameObject.GetInstanceID(), clickAction);
            deviceUnclickAction.Add(deviceOrderInfo.device.gameObject.GetInstanceID(), unclickAction);
        }

        void UnRegisterClickEvent(DeviceOrderInfo deviceOrderInfo)
        {
            Assert.IsNotNull(deviceOrderInfo.device);
            Assert.IsNotNull(deviceOrderInfo.device);
            int id = deviceOrderInfo.device.gameObject.GetInstanceID();
            
            Assert.IsTrue(deviceClickActions.ContainsKey(id));
            Assert.IsTrue(deviceUnclickAction.ContainsKey(id));

            
            deviceOrderInfo.device.OnDeviceSelected.RemoveListener(deviceClickActions[id]);
            deviceOrderInfo.device.OnDeviceUnselected.RemoveListener(deviceUnclickAction[id]);

            deviceClickActions.Remove(id);
            deviceUnclickAction.Remove(id);
        }


        private int m_selectedDeviceIdx = -1;
        void DeviceOnClick(int idx)
        {
            m_selectedDeviceIdx = 1;
        }

        void DeviceOnUnClick(int idx)
        {
            m_selectedDeviceIdx = -1;
        }

        public bool removeSelectedDevice()
        {
            if (m_selectedDeviceIdx == - 1) return false;

            RemoveDevice(m_selectedDeviceIdx);

            return true;
        }
    }
}
