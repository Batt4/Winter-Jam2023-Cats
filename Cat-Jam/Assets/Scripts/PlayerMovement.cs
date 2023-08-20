using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Player Life")]
    public float playerHP;
    public float startingHp = 3;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDir;
    Rigidbody rb;

    [Header("Jump")]
    public float basejumpForce;
    public float jumpForceIncrement;
    public float maxJumpForce;
    float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;




    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        grounded = true;
        jumpForce = basejumpForce;
        playerHP = startingHp;

    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position+ new Vector3(0,playerHeight * 0.5f,0), Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);

        myInput();
        speedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

    }
    private void FixedUpdate()
    {
        movePlayer();
    }

    private void myInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && jumpForce <= maxJumpForce)// && rb.velocity.magnitude == 0)
        {
            jumpForce += jumpForceIncrement * Time.deltaTime;
            Debug.Log("super jump: "+ jumpForce);
        }
            

        if (Input.GetKeyUp(jumpKey) && readyToJump && grounded)
        {
            Debug.Log("Jump");
            readyToJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

    }

    private void movePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void speedControl()
    {
        Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVal.magnitude > moveSpeed)
        {
            Vector3 limitedVal = flatVal.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVal.x, rb.velocity.y, limitedVal.z);
        }
    }

    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        jumpForce = basejumpForce;

    }
    private void resetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.CompareTag("Coin") || Input.GetMouseButtonDown(0))){
            Destroy(other.gameObject);
            // spawn coin on top of the cat
        }
    }

}
