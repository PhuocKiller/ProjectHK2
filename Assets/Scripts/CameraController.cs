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
        CinemachineCore.GetInputAxis = GetAxisCustom;

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


    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetKey("mouse 0"))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetKey("mouse 0"))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}
