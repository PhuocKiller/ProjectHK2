using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook virtualCamera;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineFreeLook>();
        Singleton<CinemachineBrain>.Instance.m_ShowDebugText = true;
    }

    public void SetFollowCharacter(Transform characterTransform)
    {
        virtualCamera.Follow = characterTransform;
        virtualCamera.LookAt = characterTransform;
    }
    public void RemoveFollowCharacter()
    {
        virtualCamera.Follow = null;
    }
}
