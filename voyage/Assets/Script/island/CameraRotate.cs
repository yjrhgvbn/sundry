using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotate : MonoBehaviour {

    private Transform LookAtTargetPos;

    private void Awake()
    {
        LookAtTargetPos = GameObject.FindWithTag(Tags.land).transform;
    }
    
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(1))
            {

                transform.RotateAround(LookAtTargetPos.localPosition, Vector3.up, 2f * Input.GetAxis("Mouse X"));
                Quaternion originalRotation = transform.rotation;
                Vector3 originalPos = transform.position;
                transform.RotateAround(LookAtTargetPos.localPosition, transform.right, -2f * Input.GetAxis("Mouse Y"));
                float x = transform.eulerAngles.x;
                if (x <= 10 || x >= 80)
                {
                    transform.rotation = originalRotation;
                    transform.position = originalPos;
                }
            }
            Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * -10f;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 15, 60);
        }
    }
}
