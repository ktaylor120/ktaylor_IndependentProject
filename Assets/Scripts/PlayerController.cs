using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //rate of movement
    public float speed = 30f;
    public float mouseSensitivity = 10f;

    public Camera playerCamera;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.deltaTime);
        //moving the player
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        characterController.Move(moveDirection * Time.deltaTime);

        // mouse rotate look
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

    }
}
