using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    Vector2 movement;
    Vector2 targetPosition;
    bool isMovingByMouse = false;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nhấn chuột → dùng chuột
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMovingByMouse = true;
        }

        // Nhấn phím → ưu tiên bàn phím
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            movement = new Vector2(h, v).normalized;
            isMovingByMouse = false;
        }
        else if (isMovingByMouse)
        {
            movement = (targetPosition - rb.position).normalized;

            if (Vector2.Distance(rb.position, targetPosition) <= 0.1f)
            {
                isMovingByMouse = false;
            }
        }
        else
        {
            movement = Vector2.zero;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
