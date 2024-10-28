using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook freeLookCamera;
    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        Singleton<CinemachineBrain>.Instance.m_ShowDebugText = true;
    }

    public void SetFollowCharacter(Transform characterTransform)
    {
        freeLookCamera.Follow = characterTransform;
        freeLookCamera.LookAt = characterTransform;
    }
    public void RemoveFollowCharacter()
    {
        freeLookCamera.Follow = null;
    }
}
