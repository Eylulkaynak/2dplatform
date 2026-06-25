using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Yere Basma Kontrolu")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    void Start()
    {
        // Karakterdeki Rigidbody2D bileşenini alıyoruz
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. SADECE A VE D TUŞLARINI KONTROL ET
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;  // Sağa git
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; // Sola git
        }

        // 2. YERE BASIP BASMADIĞINI KONTROL ET
        // (Zıpladıktan sonra karakterin havada sonsuza kadar zıplamasını engeller)
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // 3. SADECE SPACE TUŞU İLE ZIPLAMA
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Fizik motoru ile karakteri sağa sola yürüt (Mevcut düşme/zıplama hızını korur)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // Unity Editöründe yer kontrol kutusunu kırmızı bir çizgi olarak gösterir (Görsel kolaylık için)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
