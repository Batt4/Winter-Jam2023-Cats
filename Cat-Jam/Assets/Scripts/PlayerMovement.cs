using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    float jumpForceIncrement;
    public float maxJumpTime;
    public float maxJumpForce;
    float jumpForceDif;
    float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Animator animator;
    public float speedAnim = 1.5f;

    Vector3 initialScale;
    [Header("CAMBIAR SOLO Y, X=1, Z=1")]
    public Vector3 targetScale;
    bool isInAir = false;
    float elapsedTimeJump = 0;

    public KeyCode meowKey = KeyCode.F;

    [Header("Score UI")]
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI maxScoreTxt;
    public int maxScore = 5;


    void Start()
    {
        maxScoreTxt.text = maxScore.ToString();
        scoreTxt.text = "0";
        jumpForceDif = maxJumpForce - basejumpForce;
        jumpForceIncrement = jumpForceDif / maxJumpTime;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        grounded = true;
        jumpForce = basejumpForce;
        playerHP = startingHp;
        animator.SetFloat("speedAnim", speedAnim);
        initialScale = transform.localScale;
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

        if (Input.GetKeyUp(meowKey))
            meow();

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
            elapsedTimeJump += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTimeJump / maxJumpTime);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, alpha);
        }


        if (Input.GetKeyUp(jumpKey) && readyToJump && grounded)
        {
            elapsedTimeJump = 0;
            readyToJump = false;
            StartCoroutine(UpJumpAnim());
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
            isInAir = true;
        }

    }

    IEnumerator UpJumpAnim()
    {
        float timeToTop = CalculateTimeToTop(jumpForce);
        Debug.Log("timeToTop " + timeToTop);
        Vector3 init = transform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < timeToTop)
        {
            elapsedTime += Time.deltaTime;
            float lerpAmount = Mathf.Clamp01(elapsedTime / timeToTop);
            transform.localScale = Vector3.Lerp(init, targetScale, lerpAmount);
        }
        //StartCoroutine(ResetAnim());
        yield return null;
        
    }
    IEnumerator ResetAnim()
    {
        float timeToTop = 1.75f;
        float elapsedTime = 0f;
        Vector3 init = transform.localScale;
        while (!grounded)
        {

        }
        while (elapsedTime < timeToTop/2)
        {
            elapsedTime += Time.deltaTime;
            float lerpAmount = Mathf.Clamp01(elapsedTime / timeToTop);
            transform.localScale = Vector3.Lerp(init, targetScale, lerpAmount);
        }
        init = transform.localScale;
        while (elapsedTime < timeToTop)
        {
            elapsedTime += Time.deltaTime;
            float lerpAmount = Mathf.Clamp01(elapsedTime / timeToTop);
            transform.localScale = Vector3.Lerp(init, initialScale, lerpAmount);
            
        }
        isInAir = false;
        yield return null;
    }

    private float CalculateTimeToTop(float jumpForce)
    {
        float g = -Physics.gravity.y;
        float timeToTop = jumpForce / g;
        return timeToTop;
    }

    public void meow()
    {
        AudioManager inst = AudioManager.instance;
        inst.playFxSound(inst.meow);
    }

    private void movePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float inputMagnitude = Mathf.Clamp01(moveDir.magnitude);
        if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //inputMagnitude *= speedUpAnimOnAir;
        }

        animator.SetFloat("input", inputMagnitude, 0.05f, Time.deltaTime);
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

        //animator.SetFloat("speedAnim", 4, 0.05f, Time.deltaTime);

    }
    private void resetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Coin")){
            Destroy(other.gameObject);
            // spawn coin on top of the cat
            int i = animator.GetInteger("coins");
            Debug.Log("coins collected: " + i);
            i++;
            animator.SetInteger("coins", i);

        }
    }

}
