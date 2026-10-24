using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [Header("Player Components")]
    public Rigidbody2D rb;
    public SpriteRenderer renderer;
    public Animator animator;

    [Header("Player Health")]
    //[SerializeField] private int health = 100;
    [SerializeField] private float invincibilityTime = 1f;

    private float nextDamageTime;

    [Header("Player Controls")]
    public float moveSpeed = 5f;
    

    //Dynamic Jumping
    bool isJumpPressed;
    [Header("Jump Controls")]
    public float jumpForce = 5f;
    public float lowJumpMultiplier = 2.5f;
    public float fallMultiplier = 3f;


    bool isGrounded = false;
    public GameObject groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    bool isFacingRight = true;

    bool playerHit = false;

    Vector2 moveInput;

    private Coroutine coroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        AudioManager.Instance.PlayMusic("MainMusic", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundLayer);
        if (!playerHit)
        {
            MovePlayer();
        }
        ApplyDynamicGravity();
        animator.SetFloat("yVelocity",rb.linearVelocityY);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        isJumpPressed = value.isPressed;
        if (isGrounded && isJumpPressed)
        {
            AudioManager.Instance.PlaySFX("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
    }

    void MovePlayer()
    {
        if(moveInput.x !=0)
        {
            animator.SetBool("IsMove",true);
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
        if ((moveInput.x > 0 && !isFacingRight) || (moveInput.x < 0 && isFacingRight))
        {
            FlipPlayer();
        }

        rb.linearVelocityX = moveInput.x * moveSpeed;
    }

    void FlipPlayer()
    {
        isFacingRight = !isFacingRight;

        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void ApplyDynamicGravity()
    {
        if(rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocityY > 0 && !isJumpPressed)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void TakeDamage()
    {

        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + invincibilityTime;
        animator.SetTrigger("onHit");
        ApplyImpactForce();
        playerHit = true;
        StartCoroutine(ImapactReset());
        if (coroutine != null) StopCoroutine(coroutine);
    }

    void ApplyImpactForce()
    {
        
        if (isFacingRight)
        {
            Vector2 impactForce = new Vector2(-10f, 20f);
            rb.AddForce(impactForce, ForceMode2D.Impulse);
        }
        else
        {
            Vector2 impactForce = new Vector2(10f, 20f);
            rb.AddForce(impactForce, ForceMode2D.Impulse);
        }
        
    }

    IEnumerator ImapactReset()
    {
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;
        playerHit = false;
    }

    public void PlayWalkAudio()
    {
        AudioManager.Instance.PlaySFX("Run");
    }
}
