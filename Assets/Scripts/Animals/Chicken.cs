using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float speed = 2f;

    private Vector2 targetPosition;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetNewTarget();
    }

    void FixedUpdate()
    {
        Vector2 direction = targetPosition - rb.position;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            speed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        // Quay mặt theo hướng đi
        if(direction.x > 0)
            {
                 transform.localScale = new Vector3(-1, 1, 1);
            }
        else if(direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

        // Tới nơi → đổi chỗ mới
        if(Vector2.Distance(rb.position, targetPosition) < 0.2f)
        {
            SetNewTarget();
        }
    }
    void SetNewTarget()
    {
        float randomX = Random.Range(22f, 30f);
        float randomY = Random.Range(-12f, -2.5f);

        targetPosition = new Vector2(randomX, randomY);
    }
}
