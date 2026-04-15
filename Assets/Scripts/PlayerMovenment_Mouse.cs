using UnityEngine;

public class PlayerMovenment_Mouse : MonoBehaviour
{
   public Rigidbody2D rb;
    public float moveSpeed = 5f;
    Vector2 targetPosition;//Lưu lại vị trí con trỏ chuột trên màn hình
    Vector2 movement;
    bool isMoving = false;//Biến để kiểm tra xem người chơi có đang di chuyển hay không
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))//Kiểm tra nếu người chơi nhấn chuột trái
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Chuyển đổi vị trí
            isMoving = true;//Bật cờ di chuyển khi người chơi nhấn chuột trái
        }
        
       
        movement=(targetPosition - rb.position).normalized;//Tính toán hướng di chuyển từ vị trí hiện tại đến vị trí mục tiêu
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", isMoving?1:0);//Đặt tốc độ hoạt ảnh dựa trên trạng thái di chuyển (1 nếu đang di chuyển, 0 nếu không)
       
    }

    void FixedUpdate()
    {
        if(isMoving)//Nếu người chơi đang di chuyển
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            if(Vector2.Distance(rb.position, targetPosition) <= 0.1f)//Kiểm tra nếu người chơi đã đến gần vị trí mục tiêu
            {
            isMoving = false;//Tắt cờ di chuyển khi người chơi đến gần vị trí mục tiêu
            Debug.Log("Đã đến vị trí mục tiêu!");//In ra thông báo khi người chơi đến vị trí mục tiêu
            }
        }
        
    }

}
