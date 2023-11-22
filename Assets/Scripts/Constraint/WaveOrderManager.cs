

using GO_Device;
using GO_Wave;
using Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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
            m_waveDeviceOrder.removeDevice(deviceIdx);

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


        void WaitForOneFixedUpdateAndTrigger(I_ParameterTransfer i_ParameterTransfer, Action extraBehavior)
        {
            StartCoroutine(WaitForOneFixedUpdateAndTriggerCoroutine(i_ParameterTransfer, extraBehavior));
        }

        IEnumerator WaitForOneFixedUpdateAndTriggerCoroutine(I_ParameterTransfer i_ParameterTransfer, Action extraBehavior)
        {
            yield return new WaitForFixedUpdate();

            i_ParameterTransfer.ParameterChangeTrigger();
            extraBehavior?.Invoke();
        }
    }
}
