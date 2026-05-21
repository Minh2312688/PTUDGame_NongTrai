using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    Vector2 movement;
    Vector2 targetPosition;
    bool isMovingByMouse = false;
    public Animator animator;
    public Vector2 minPos; // góc trái dưới map
    public Vector2 maxPos; // góc phải trên map
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Chặn di chuyển khi đang nhập username hoặc chuột trên UI
        bool isUiPointer = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        // Chỉ chặn hoàn toàn khi username wizard mở
        if (UsernameWizard.IsUsernameWizardOpen)
        {
            movement = Vector2.zero;
            isMovingByMouse = false;

            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Chỉ cho click move khi inventory đóng
            if (!GameManager.Instance.uiManager.inventoryPanel.activeSelf)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isMovingByMouse = true;
            }
        }

        // WASD luôn hoạt động
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

        if (movement.sqrMagnitude > 0)
        {
            AudioManager.Instance.PlayWalk();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        
        // Chặn di chuyển trong giới hạn map
        newPosition.x = Mathf.Clamp(newPosition.x, minPos.x, maxPos.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPos.y, maxPos.y);
        
        rb.MovePosition(newPosition);
    }
}
