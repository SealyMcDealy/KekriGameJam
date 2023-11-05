using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float extinguisherKnockback;
    [SerializeField] private float sensitivity;
    [SerializeField] private float fireKnockback;
    [SerializeField] private float speedLimit;


    private AudioSource fireExtinguisherSound;
    private ParticleSystem fireExtinguisherParticles;
    private Camera PlayerCam;
    private Rigidbody rb;
    private float ver;
    private float hor;


    //public List<AudioClip> clipList;
    //private AudioSource audioSource;


    void Start()
    {
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        fireExtinguisherParticles = GetComponentInChildren<ParticleSystem>();
        fireExtinguisherParticles.Stop();

        fireExtinguisherSound = GetComponentInChildren<AudioSource>();
        fireExtinguisherSound.Stop();

        //audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        CameraMover();


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
        
        Vector3 rot;
        rot = forward * -Input.GetAxis("Horizontal") * rotationSpeed;
        transform.Rotate(0, rot.y, 0);

        // above: gets the rotation relative to the camera

        //limit velocity if going over the limit

        if (rb.velocity.magnitude >= speedLimit)
        {
            rb.AddForce(transform.position * -(rb.velocity.magnitude - speedLimit));
        }

    }

    void CameraMover() //not in use yet
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        PlayerCam.transform.RotateAround(transform.position, -Vector3.up, rotateHorizontal * sensitivity); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
        PlayerCam.transform.RotateAround(Vector3.zero, transform.right, rotateVertical * sensitivity); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player
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
        if (collision.gameObject.CompareTag("Fire"))
        {
            Vector3 direction = transform.position - collision.transform.position;

            rb.AddForce((direction * fireKnockback) + new Vector3(0, jumpHeight * 0.5f, 0));


            //if (audioSource.isPlaying == false)
            //{
            //    int some = Random.Range(0, clipList.Count);
            //    audioSource.clip = clipList[some];
            //    audioSource.Play();
            //}
            

            Debug.Log("fire knockback");
            GameManager.IsHit = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            Vector3 direction = transform.position - other.transform.position;

            rb.AddForce((direction * fireKnockback) + new Vector3(0, jumpHeight * 0.5f, 0));

            Debug.Log("fire knockback");
            GameManager.IsHit = true;
        }
        if (other.gameObject.CompareTag("Ragdoll"))
        {
            var test = other.gameObject.GetComponent<RagdollLogic>();

            test.physicsRagdoll.SetActive(true);

            test.physicsRagdoll.transform.SetParent(null);

            test.standingRagdoll.SetActive(false);

            int some = Random.Range(0, test.voiceLines.Count);
            test.source.clip = test.voiceLines[some];
            test.source.Play();

            Destroy(test.gameObject, test.source.clip.length);
        }
    }
}
