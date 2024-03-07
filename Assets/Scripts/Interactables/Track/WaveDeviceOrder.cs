using GO_Device;
using GO_Wave;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Constraint
{
    public class WaveDeviceOrder
    {
        public class DeviceOrderInfo
        {
            public DeviceBase device;
            public int index;

            public DeviceOrderInfo(DeviceBase device, int index)
            {
                this.device = device;
                this.index = index;
            }
        }

        private readonly int MaxDeviceCount;

        private readonly int DeviceSeperationDistance;

        private List<DeviceOrderInfo> m_devices = new List<DeviceOrderInfo>();
        
        private Wave m_waveSource;

        public Wave WaveSource
        {
            get => m_waveSource;
            set => m_waveSource = value;
        }

        public int DeviceCount => m_devices.Count;

        public WaveDeviceOrder(int maxDeviceCount, int deviceSeperationDistance)
        {
            m_waveSource = null;
            MaxDeviceCount = maxDeviceCount;
            DeviceSeperationDistance = deviceSeperationDistance;
        }

        public WaveDeviceOrder(Wave waveSource, int maxDeviceCount, int deviceSeperationDistance)
        {
            m_waveSource = waveSource;
            MaxDeviceCount = maxDeviceCount;
            DeviceSeperationDistance = deviceSeperationDistance;
        }

        public bool AppendDevice(DeviceBase device)
        {
            if (m_devices.Count < MaxDeviceCount)
            {
                m_devices.Add(new DeviceOrderInfo(device, m_devices.Count));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddDevice(DeviceBase device, int index)
        {
            if (m_devices.Count == MaxDeviceCount) return false;
            Assert.IsTrue(m_devices.Count > MaxDeviceCount);

            Assert.IsTrue(index >= 0 && index < MaxDeviceCount);

            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            Assert.IsNotNull(device);

            m_devices.Insert(index, new DeviceOrderInfo(device, index));

            // update each device's index
            for (int i = index + 1; i < m_devices.Count; i++)
            {
                m_devices[i].index = i;
            }

            return true;
        }

        public bool ReplaceDevice(DeviceOrderInfo device, int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            Assert.IsNotNull(device);

            m_devices[index] = device;
            device.index = index;
            

            return true;
        }

        public bool removeDevice(int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            m_devices.RemoveAt(index);

            for(int i = index; i < m_devices.Count; i++)
            {
                m_devices[i].index = i;
            }

            return true;
        }

        public DeviceBase GetDevice(int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            return m_devices[index].device;
        }

        public DeviceOrderInfo GetDeviceOrderInfo(int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            return m_devices[index];
        }

        public Vector3 GetDevicePosition(int index)
        {
            Assert.IsNotNull(m_waveSource);
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            return m_waveSource.transform.position + m_waveSource.transform.forward * DeviceSeperationDistance * (index + 1);
        }

        public int GetDevicePositionIndex(Vector3 position, int originalIdx)
        {
            Assert.IsNotNull(m_waveSource);
            var origPosition = GetDevicePosition(originalIdx);

            

            Vector3 sourcePosition = m_waveSource.transform.position;

            

            Vector3 direction = m_waveSource.transform.forward;

            Vector3 relativePosition = position - sourcePosition;

            float distance = Vector3.Dot(relativePosition, direction);
            float origDistance = Vector3.Dot(origPosition - sourcePosition, direction);

            int index;
            if (distance > origDistance)
            {
                index = (int)Mathf.Floor(distance / DeviceSeperationDistance) - 1;
            }
            else
            {
                index = (int)Mathf.Ceil(distance / DeviceSeperationDistance) - 1;
            }

            if (index < 0) index = 0;

            return index;
        }
    }
}

