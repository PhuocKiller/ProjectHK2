using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDirection : MonoBehaviour
{
    [SerializeField] GameObject ImageParent;
    public Image skillImage; // Kéo thả hình ảnh vào đây trong Inspector
    public float skillDuration = 0.5f; // Thời gian kỹ năng
    Vector3 direction, directionNormalize, fixPosition;
    PlayerController player;
    private void Start()
    {
        player= ImageParent.transform.parent.parent.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            fixPosition = ImageParent.transform.position;
        }
        if (Input.GetMouseButton(1))
        {
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
            // ImageParent.transform.position = fixPosition;
            ImageParent.transform.position = hitInfo.point - 0.5f * direction;
            player.state = 5; //trạng thái xài skill ko cho di chuyển
            ImageParent.GetComponentInChildren<Image>().enabled = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            ImageParent.transform.position = fixPosition;
            ImageParent.GetComponentInChildren<Image>().enabled = false;
            player.state = 0; //trạng thái tự do thì cho di chuyển
            //ImageParent.transform.position=fixPosition;
        }
    }
}
