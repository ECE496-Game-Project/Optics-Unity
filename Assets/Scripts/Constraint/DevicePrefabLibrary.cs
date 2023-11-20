using CommonUtils;
using GO_Device;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DevicePrefabLibrary : MonoSingleton<DevicePrefabLibrary>
{
    [SerializeField] private List<string> m_deviceName;
    [SerializeField] private List<DeviceBase> m_devicePrefab;

    private Dictionary<string, DeviceBase> m_deviceNameDict = new Dictionary<string, DeviceBase>();
    private Dictionary<DEVICETYPE, DeviceBase> m_deviceEnumDic = new Dictionary<DEVICETYPE, DeviceBase>();
    private Dictionary<string, DEVICETYPE> m_deviceNameEnumDic = new Dictionary<string, DEVICETYPE>();
    private Dictionary<DEVICETYPE, string> m_deviceEnumNameDic = new Dictionary<DEVICETYPE, string>();

    protected override void Init()
    {
        Assert.IsTrue(m_deviceName.Count == m_devicePrefab.Count);

        for (int i = 0; i < m_deviceName.Count; i++)
        {
            Assert.IsFalse(m_deviceNameDict.ContainsKey(m_deviceName[i]));

            m_deviceNameDict.Add(m_deviceName[i], m_devicePrefab[i]);

            Assert.IsFalse(m_deviceEnumDic.ContainsKey(m_devicePrefab[i].type));

            m_deviceEnumDic.Add(m_devicePrefab[i].type, m_devicePrefab[i]);

            m_deviceNameEnumDic.Add(m_deviceName[i], m_devicePrefab[i].type);

            m_deviceEnumNameDic.Add(m_devicePrefab[i].type, m_deviceName[i]);
        }
    }

    public DeviceBase CreateDevice(string name)
    {
        Assert.IsTrue(m_deviceNameDict.ContainsKey(name));

        return Instantiate(m_deviceNameDict[name]);
    }

    public DeviceBase CreateDevice(DEVICETYPE type)
    {
        Assert.IsTrue(m_deviceEnumDic.ContainsKey(type));

        return Instantiate(m_deviceEnumDic[type]);
    }

    public DEVICETYPE GetDeviceType(string name)
    {
        Assert.IsTrue(m_deviceNameEnumDic.ContainsKey(name));

        return m_deviceNameEnumDic[name];
    }

    public string GetDeviceName(DEVICETYPE type)
    {
        Assert.IsTrue(m_deviceEnumNameDic.ContainsKey(type));

        return new string(m_deviceEnumNameDic[type]);
    }
}
