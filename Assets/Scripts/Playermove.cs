using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("起點與終點")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("勝利文字")]
    [SerializeField] private TMP_Text victoryText;

    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("左右移動邊界：陣列需填入兩個 X 座標")]
    [SerializeField] private float[] horizontalLimits = { -10f, 10f };

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool isGrounded;
    private bool hasWon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (startPoint != null)
        {
            rb.position = startPoint.position;
        }

        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasWon)
            return;

        moveInput = Input.GetAxisRaw("Horizontal");

        // 向左移動時翻轉圖片
        if (moveInput < 0f)
            spriteRenderer.flipX = true;
        else if (moveInput > 0f)
            spriteRenderer.flipX = false;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float jumpVelocity = Mathf.Sqrt(2f * gravity * jumpHeight);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (hasWon)
            return;

        // Vector2 水平移動
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;

        // 用陣列限制左右移動範圍
        if (horizontalLimits != null && horizontalLimits.Length >= 2)
        {
            float minX = Mathf.Min(horizontalLimits[0], horizontalLimits[1]);
            float maxX = Mathf.Max(horizontalLimits[0], horizontalLimits[1]);

            Vector2 position = rb.position;
            position.x = Mathf.Clamp(position.x, minX, maxX);
            rb.position = position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasWon || endPoint == null)
            return;

        // 只有碰到指定的 End 物件才獲勝
        if (other.transform == endPoint || other.transform.IsChildOf(endPoint))
        {
            hasWon = true;
            rb.linearVelocity = Vector2.zero;

            if (victoryText != null)
            {
                victoryText.gameObject.SetActive(true);
                victoryText.text = "Victory!";
            }
        }
    }
}