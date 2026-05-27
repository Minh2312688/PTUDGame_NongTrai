using UnityEngine;

public class Chicken : MonoBehaviour
{
    // Tốc độ đi
    public float speed = 2f;

    // Điểm muốn tới
    private Vector3 targetPosition;

    // Vị trí ban đầu
    private Vector3 startPosition;

    // Script ăn
    private ChickenEat chickenEat;

    void Start()
    {
        // Lưu vị trí gốc
        startPosition = transform.position;

        // Lấy script ChickenEat
        chickenEat =
            GetComponent<ChickenEat>();

        // Chọn điểm random đầu tiên
        SetNewTarget();
    }

    void Update()
    {
        // Nếu đang ăn thì đứng yên
        if(chickenEat != null &&
            chickenEat.IsEating())
        {
            return;
        }

        // Di chuyển tới điểm target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime);

        // Nếu gần tới nơi
        if(Vector3.Distance(
            transform.position,
            targetPosition) < 0.2f)
        {
            // Chọn vị trí mới
            SetNewTarget();
        }

        // Quay mặt đúng hướng
        FlipChicken();
    }

    void SetNewTarget()
    {
        // Random trong hàng rào
        float randomX =
            Random.Range(-3.5f, 3.5f);

        float randomY =
            Random.Range(-4.5f, 4.5f);

        // Tính vị trí mới
        targetPosition =
            startPosition +
            new Vector3(
                randomX,
                randomY,
                0);
    }

    void FlipChicken()
    {
        // Đi phải
        if(targetPosition.x >
            transform.position.x)
        {
            transform.localScale =
                new Vector3(-1,1,1);
        }
        // Đi trái
        else
        {
            transform.localScale =
                new Vector3(1,1,1);
        }
    }
}