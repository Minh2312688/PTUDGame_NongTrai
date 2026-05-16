using UnityEngine;

public class Pig : MonoBehaviour
{
    // Tốc độ đi
    public float speed = 1.5f;

    // Điểm muốn tới
    private Vector3 targetPosition;

    // Vị trí ban đầu
    private Vector3 startPosition;

    void Start()
    {
        // Lưu vị trí gốc
        startPosition = transform.position;

        // Chọn điểm random đầu tiên
        SetNewTarget();
    }

    void Update()
    {
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
        FlipPig();
    }

    void SetNewTarget()
    {
        // Random trong hàng rào
        float randomX =
            Random.Range(-4f, 4f);

        float randomY =
            Random.Range(-5f, 5f);

        // Tính vị trí mới
        targetPosition =
            startPosition +
            new Vector3(
                randomX,
                randomY,
                0);
    }

    void FlipPig()
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