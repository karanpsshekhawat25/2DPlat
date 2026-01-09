using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Transform respawnPoint;
    public float fallY = -10f;

    Rigidbody2D rb;
    Animator anim;
    bool isGrounded;

    Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        originalScale = transform.localScale;
        Time.timeScale = 1f;
    }

    void Update()
    {
        Move();
        Jump();
        CheckFall();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(h) > 0.1f)
        {
            anim.SetBool("isWalking", true);

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (h > 0 ? 1 : -1);
            transform.localScale = scale;
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void CheckFall()
    {
        if (transform.position.y < fallY)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.SetParent(null);
        rb.velocity = Vector2.zero;
        transform.position = respawnPoint.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    transform.SetParent(collision.transform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            transform.SetParent(null);
        }
    }
}
