using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float extinguisherKnockback;
    [SerializeField] private float sensitivity;


    private AudioSource fireExtinguisherSound;
    private ParticleSystem fireExtinguisherParticles;
    private Camera PlayerCam;
    private Rigidbody rb;
    private float ver;
    private float hor;

    void Start()
    {
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        fireExtinguisherParticles = GetComponentInChildren<ParticleSystem>();
        fireExtinguisherParticles.Stop();

        fireExtinguisherSound = GetComponentInChildren<AudioSource>();
        fireExtinguisherSound.Stop();
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        CameraMover();

        Debug.Log("noise playing " + fireExtinguisherSound.isPlaying);
        if (Input.GetKey(KeyCode.Space) && IsGrounded == true)
        {
            Jumper();
        }
        if (Input.GetKey(KeyCode.E))
        {
            fireExtinguisherParticles.Play();

            if (fireExtinguisherSound.isPlaying == false)
            {
                fireExtinguisherSound.Play();
            }

            Blower();
        }
        else
        {
            
            if (fireExtinguisherSound.isPlaying == true)
            {
                fireExtinguisherSound.Stop();
            }
            fireExtinguisherParticles.Stop();
        }
    }

    void FixedUpdate()
    {
        Mover();
    }

    void Mover()
    {
        //get cam values
        var forward = PlayerCam.transform.forward;
        var right = PlayerCam.transform.right;

        //get camera's horizontal values only 
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //set movement values
        var desiredMoveDirection = forward * ver + right * hor;
        //normalize movement values so that diagonal movement is not faster
        desiredMoveDirection = Vector3.ClampMagnitude(desiredMoveDirection, 1);
        rb.velocity = new Vector3(desiredMoveDirection.x * speed * Time.deltaTime, rb.velocity.y, desiredMoveDirection.z * speed * Time.deltaTime);

    }

    void CameraMover()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        transform.RotateAround(transform.position, -Vector3.up, rotateHorizontal * sensitivity); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
        transform.RotateAround(Vector3.zero, transform.right, rotateVertical * sensitivity); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player
    }

    void Jumper()
    {
        IsGrounded = false;
        rb.AddForce(0, jumpHeight, 0);
    }

    void Blower()
    {
        rb.AddRelativeForce(0, 0, -extinguisherKnockback);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }
}
