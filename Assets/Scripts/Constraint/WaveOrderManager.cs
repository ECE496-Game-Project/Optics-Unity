

using GO_Device;
using GO_Wave;
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

            m_waveDeviceOrder.WaveSource = waveSource.transform;

            // assume the rest of the children are my wave devices
            for (int i = 1; i < transform.childCount; i++)
            {
                var waveDevice = transform.GetChild(i)?.GetComponent<DeviceBase>();
                Assert.IsNotNull(waveDevice);

                m_waveDeviceOrder.AppendDevice(waveDevice);
            }

            SetDevicePositions();
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
            var firstDevice = m_waveDeviceOrder.GetDevice(firstDeviceIdx);

            var secondDevice = m_waveDeviceOrder.GetDevice(secondDeviceIdx);

            m_waveDeviceOrder.ReplaceDevice(firstDevice, secondDeviceIdx);
            m_waveDeviceOrder.ReplaceDevice(secondDevice, firstDeviceIdx);

            SetDevicePositions();

        }

        public void RemoveDevice(int deviceIdx)
        {
            var device = m_waveDeviceOrder.GetDevice(deviceIdx);
            m_waveDeviceOrder.removeDevice(deviceIdx);

            Destroy(device.gameObject);

            SetDevicePositions();
        }

        public void AddDevice(string deviceType)
        {

        }
    }
}
