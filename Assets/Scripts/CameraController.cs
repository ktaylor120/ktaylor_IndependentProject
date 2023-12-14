using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float YMin = -50.0f;
    private const float YMax = 30.0f;

    public Transform lookAt;
    public Transform player;

    public float minDistance = 3.0f;
    public float maxDistance = 10.0f;
    public float sensitivity = 2.0f;
    public float scrollSpeed = 10.0f; 
    public float yOffset = 5.0f; 

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float distance = 8.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY += Input.GetAxis("Mouse Y") * sensitivity;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        currentY = Mathf.Clamp(currentY, YMin, YMax);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Vector3 direction = new Vector3(0, -yOffset, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position - rotation * direction;

        transform.LookAt(lookAt.position);
    }
}