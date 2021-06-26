using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpead, zoomSpeed;

    public float minZoomDis, maxZoomDis;

    private Camera cam;

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
        cam = Camera.main;
    }

    private void Update()
    {
        move();
        Zoom();
    }

    //moves the camera with the "WASD" keys as well as arrow keys, this is a built in function with unity
    void move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * zInput + transform.right * xInput;

        transform.position += dir * moveSpead * Time.deltaTime;
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDis && scrollInput > 0.0f)
        {
            return;
        }
        else if (dist > maxZoomDis && scrollInput < 0.0f)
            return;

        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }

    public void FocusOnPosition (Vector3 pos)
    {
        transform.position = pos;
    }


}
