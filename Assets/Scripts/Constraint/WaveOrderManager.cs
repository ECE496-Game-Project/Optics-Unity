

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

            WaitForOneFixedUpdateAndTrigger(waveSource);
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
            
            int highestIdx = (firstDeviceIdx > secondDeviceIdx) ? firstDeviceIdx : secondDeviceIdx;

            var firstDevice = m_waveDeviceOrder.GetDevice(firstDeviceIdx);

            var secondDevice = m_waveDeviceOrder.GetDevice(secondDeviceIdx);

            m_waveDeviceOrder.ReplaceDevice(firstDevice, secondDeviceIdx);
            m_waveDeviceOrder.ReplaceDevice(secondDevice, firstDeviceIdx);

            SetDevicePositions();

            //might also need to update gameobject hierarchy

            // the device which is currently in has the lower hierarchy was the device that has the higher hierarchy
            // we need to notify this device's parent wave source that need to regenerate
            var lowerHierarchyDevice = m_waveDeviceOrder.GetDevice(highestIdx);

            var deviceParameterTransfer = (I_ParameterTransfer)lowerHierarchyDevice;

            Assert.IsNotNull(deviceParameterTransfer);

            WaitForOneFixedUpdateAndTrigger(deviceParameterTransfer, null);


        }

        public void RemoveDevice(int deviceIdx)
        {
            var device = m_waveDeviceOrder.GetDevice(deviceIdx);
            m_waveDeviceOrder.removeDevice(deviceIdx);

            device.gameObject.GetComponent<Collider>().enabled = false;



            SetDevicePositions();

           
            // Destroy the object after param is trigger
            WaitForOneFixedUpdateAndTrigger((I_ParameterTransfer) device, 
                () => { Destroy(device.gameObject); });
        }

        public void AddDevice(string deviceType)
        {

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
