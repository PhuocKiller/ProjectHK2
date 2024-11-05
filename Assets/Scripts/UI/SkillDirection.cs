using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDirection : MonoBehaviour
{
    [SerializeField] GameObject ImageParent;
    public Image skillImage; // Kéo thả hình ảnh vào đây trong Inspector
    public float skillDuration = 0.5f; // Thời gian kỹ năng
    public Vector3 direction, directionNormalize, fixPosition;
    PlayerController player;
    private void Start()
    {
        player= ImageParent.transform.parent.parent.GetComponent<PlayerController>();
    }
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            GetMouseDown();
        }
        if (Input.GetMouseButton(0))
        {
            GetMouse();
        }
        if (Input.GetMouseButtonUp(0))
        {
            GetMouseUp();
        }*/
    }

    public void GetMouseUp()
    {
        //  if (player.state != 5) return;
       // player.state = 0;
        ImageParent.transform.position = fixPosition;
        ImageParent.GetComponentInChildren<Image>().enabled = false;
    }

    public void GetMouse()
    {
        if (player.state != 5) return;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200))
        {
            direction = hitInfo.point - fixPosition;
            directionNormalize = direction.normalized;
        }

        ImageParent.transform.rotation = Quaternion.LookRotation(direction);
        ImageParent.transform.localScale = new Vector3(1, 1, 1.85f * (hitInfo.point - fixPosition).magnitude);
        ImageParent.transform.position = hitInfo.point - 0.5f * direction;
        ImageParent.GetComponentInChildren<Image>().enabled = true;
    }

    public void GetMouseDown()
    {
       // if (player.state != 5) return;
        fixPosition = ImageParent.transform.position;
    }
}
