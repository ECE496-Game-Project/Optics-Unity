using GO_Device;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Constraint
{
    public class WaveDeviceOrder
    {
        private readonly int MaxDeviceCount;

        private readonly int DeviceSeperationDistance;

        private List<DeviceBase> m_devices = new List<DeviceBase>();

        private Transform m_waveSource;

        public Transform WaveSource
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

        public WaveDeviceOrder(Transform waveSource, int maxDeviceCount, int deviceSeperationDistance)
        {
            m_waveSource = waveSource;
            MaxDeviceCount = maxDeviceCount;
            DeviceSeperationDistance = deviceSeperationDistance;
        }

        public bool AppendDevice(DeviceBase device)
        {
            if (m_devices.Count < MaxDeviceCount)
            {
                m_devices.Add(device);
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

            m_devices.Insert(index, device);

            return true;
        }

        public bool ReplaceDevice(DeviceBase device, int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            Assert.IsNotNull(device);

            m_devices[index] = device;

            return true;
        }

        public bool removeDevice(int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            m_devices.RemoveAt(index);

            return true;
        }

        public DeviceBase GetDevice(int index)
        {
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            return m_devices[index];
        }

        public Vector3 GetDevicePosition(int index)
        {
            Assert.IsNotNull(m_waveSource);
            Assert.IsTrue(index >= 0 && index < m_devices.Count);

            return m_waveSource.position + m_waveSource.forward * DeviceSeperationDistance * (index + 1);
        }

    }
}

