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
    bool wasGrounded = true;

    public Animator animator;
    public float speedAnim = 1.5f;

    Vector3 initialScale;
    Vector3 currentScale;
    [Header("CAMBIAR SOLO Y, X=1, Z=1")]
    public Vector3 targetScale;
    bool isInAir = false;
    float elapsedTimeJump = 0;
    public float jumpRecoveryTime = 2f;
    float timeToTop = 1;

    public KeyCode meowKey = KeyCode.F;

    [Header("Score UI")]
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI maxScoreTxt;
    public int maxScore = 5;
    GameController gameController;
    public float chanceToWobble = 0.15f;
    Coroutine wobble;


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
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        rb.drag = groundDrag;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position+ new Vector3(0,playerHeight * 0.5f,0), Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);

        myInput();
        speedControl();

        if (!wasGrounded && grounded)
        {
            StartCoroutine(OscillateScale());
            wasGrounded = true;
        }
        else if (!grounded && wasGrounded)
        {
            wasGrounded = false;
        }

        if ((Random.Range(0,100) < chanceToWobble) && wobble != null)
            wobble = StartCoroutine(OscillateScale());

        
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
            currentScale = transform.localScale;
        }


        if (Input.GetKeyUp(jumpKey) && readyToJump && grounded)
        {
            elapsedTimeJump = 0;
            readyToJump = false;
            
            jump();
            StartCoroutine(UpJumpAnim());
            Invoke(nameof(resetJump), jumpCooldown);
        }

    }

    IEnumerator UpJumpAnim()
    {
        timeToTop = CalculateTimeToTop(jumpForce)/2;
        float startTime = Time.time;
        while (Time.time - startTime < timeToTop)
        {
            float t = (Time.time - startTime) / timeToTop;
            transform.localScale = Vector3.Lerp(currentScale, initialScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
        transform.localScale = initialScale;
    }

    private IEnumerator OscillateScale()
    {
        float startTime = Time.time;
        while (Time.time - startTime < jumpRecoveryTime)
        {
            float t = (Time.time - startTime) / jumpRecoveryTime;
            if ((Time.time - startTime) < jumpRecoveryTime / 2)
                transform.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            else
                transform.localScale = Vector3.Lerp(targetScale, initialScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            
            yield return null;
        }
        transform.localScale = initialScale;
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
        inst.playFxSound();
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
            AudioManager.instance.playFxSound();
            int i = animator.GetInteger("coins");
            i++;
            scoreTxt.text = i.ToString();
            animator.SetInteger("coins", i);
            if (i > gameController.numberOfSpawns)
                gameController.win = true;
        }

    }

}
