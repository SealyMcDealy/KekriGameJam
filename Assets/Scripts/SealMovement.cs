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
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space) && IsGrounded == true)
        {
            Jumper();
        }
       
        
        if (Input.GetKey(KeyCode.E))
        {
            fireExtinguisherParticles.Play();
            Blower();
        }
        else
        {
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
