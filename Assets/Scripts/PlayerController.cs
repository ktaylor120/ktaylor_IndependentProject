using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class PlayerController : MonoBehaviour
{
    CharacterController Controller;
    public Transform Cam;

    public float Speed = 10.0f;
    public GameObject ProjectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;

        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Movement.y = 0f;

        Controller.Move(Movement);

        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraController>().sensivity * Time.deltaTime);


            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        }
    }

}
