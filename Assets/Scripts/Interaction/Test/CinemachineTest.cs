using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineTest : MonoBehaviour
{

    public CinemachineVirtualCamera m_vcam;
    public CinemachineTransposer m_transposer;
    // Start is called before the first frame update
    void Start()
    {
        m_vcam = GetComponent<CinemachineVirtualCamera>();
        m_transposer = m_vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
    }

    // Update is called once per frame
    void Update()
    {
        m_transposer.m_FollowOffset.z -= 3 * Time.deltaTime;

    }
}
