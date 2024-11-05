using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook freeLookCamera;
    PlayerController player;
    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        Singleton<CinemachineBrain>.Instance.m_ShowDebugText = true;
        CinemachineCore.GetInputAxis = GetAxisCustom;
        // Lưu lại góc quay ban đầu của camera
        startRotation = freeLookCamera.transform.rotation;

        // Thiết lập góc quay cuối cùng mà camera muốn hướng đến
        
    }

    public void SetFollowCharacter(Transform transformCamera,Transform characterTransform)
    {
        freeLookCamera.Follow = transformCamera;
        freeLookCamera.LookAt = transformCamera;
        targetLookAt = transformCamera;
        player = characterTransform.gameObject.GetComponent<PlayerController>();
        endRotation = Quaternion.LookRotation(targetLookAt.position - freeLookCamera.transform.position);
    }
    public void RemoveFollowCharacter()
    {
        freeLookCamera.Follow = null;
    }


    public float GetAxisCustom(string axisName)
    {
        if(player == null) return 0f;
        if (axisName == "Mouse X")
        {
            if (Input.GetKey("mouse 0") && player.GetCurrentState()==0
                && Input.mousePosition.x>Screen.width/2)
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
            if (Input.GetKey("mouse 0") && player.GetCurrentState() == 0
                && Input.mousePosition.x > Screen.width / 2)
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

    public Transform targetLookAt;  // Tham chiếu đến đối tượng mà camera sẽ nhìn vào
    public float transitionTime = 0.01f;  // Thời gian chuyển đổi (s)

    private Quaternion startRotation;  // Vị trí quay ban đầu của camera
    private Quaternion endRotation;    // Vị trí quay mục tiêu của camera
    private float startTime;           // Thời gian bắt đầu chuyển đổi

    

    void Update()
    {
        // Kiểm tra nếu transitionTime lớn hơn 0 để bắt đầu interpolation
        if (startTime > 0)
        {
            float t = (Time.time - startTime) / transitionTime;  // Tính toán thời gian đã trôi qua
            freeLookCamera.transform.rotation = Quaternion.Slerp(startRotation,endRotation, t);  // Interpolate góc quay
        }
    }

    // Gọi hàm này khi muốn bắt đầu chuyển đổi
    public void StartTransition()
    {
        startTime = Time.time;  // Ghi lại thời gian bắt đầu
    }
}
