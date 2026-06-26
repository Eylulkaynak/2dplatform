using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))] // Karaktere otomatik Animator ekler
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Zemin Kontrolü (Ground Check)")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim; // Animasyonları kontrol edeceğimiz değişken
    private float horizontalInput;
    private bool isGrounded;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Karakterdeki Animator'ı alıyoruz
    }

    void Update()
    {
        // 1. Zemin ve Tuş Kontrolleri
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // 2. Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // 3. Her karede animasyon durumlarını güncelle
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);

        // Yüzünü döndürme (Flip)
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        
        // Zıplama anında "jump" adındaki Trigger'ı tetikle
        anim.SetTrigger("jump"); 
    }

    private void UpdateAnimations()
    {
        // Karakterin X ekseninde hareket edip etmediğini kontrol et
        bool isMoving = horizontalInput != 0;

        // Animator'a gerekli bilgileri gönderiyoruz
        anim.SetBool("isWalking", isMoving && !isRunning);
        anim.SetBool("isRunning", isMoving && isRunning);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}