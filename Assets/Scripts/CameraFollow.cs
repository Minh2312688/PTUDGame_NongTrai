using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Player
    public float smoothSpeed = 25f;
    public Vector3 offset;

    public Vector2 minPos; // góc trái dưới map
    public Vector2 maxPos; // góc phải trên map

    float camHalfHeight;
    float camHalfWidth;

    void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * Screen.width / Screen.height;
    }

    void LateUpdate()
    {
        if (target == null) return;

    Vector3 desiredPosition = target.position + offset;

    float clampX = Mathf.Clamp(desiredPosition.x, minPos.x + camHalfWidth, maxPos.x - camHalfWidth);
    float clampY = Mathf.Clamp(desiredPosition.y, minPos.y + camHalfHeight, maxPos.y - camHalfHeight);

    Vector3 clampedPosition = new Vector3(clampX, clampY, transform.position.z);

    transform.position = clampedPosition; // không dùng Lerp nữa   }

}
}