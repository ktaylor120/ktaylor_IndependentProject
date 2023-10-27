using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEditor.SceneView;

public class PlayerController : MonoBehaviour
{
    CharacterController Controller;
    public Transform Cam;

    public AudioClip punch;
    private AudioSource asPalyer;

    public bool gameOver = false;
    public float Speed = 10.0f;

    public GameObject ProjectilePrefab;
    private Animator animPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        asPalyer = GetComponent<AudioSource>();
        animPlayer = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
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

                UpdateAnimatorParameters(Horizontal, Vertical);

                transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

            }
        }

        void UpdateAnimatorParameters(float horizontal, float vertical)
        {
            float absHorizontal = Mathf.Abs(horizontal);
            float absVertical = Mathf.Abs(vertical);

            animPlayer.SetFloat("Speed", Mathf.Max(absHorizontal, absVertical));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameOver)
        {

            Instantiate(ProjectilePrefab, transform.position, transform.rotation);
            asPalyer.PlayOneShot(punch, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animPlayer.SetBool("Dead", true);
        }
    }
}
