using UnityEngine;

public class SellingShopTrigger : MonoBehaviour
{
    // Hàm tự động chạy khi Player bước vào vùng gạch của cửa hàng
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem có đúng là Player chạm vào không nhờ vào Tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Ra lệnh cho ShopUIManager mở màn hình chính lên
            if (SellingShopUIManager.Instance != null)
            {
                SellingShopUIManager.Instance.OpenMainShop();
            }
        }
    }

    // Hàm tự động chạy khi Player đi ra khỏi vùng gạch của cửa hàng
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ra lệnh ẩn màn hình chính đi
            if (SellingShopUIManager.Instance != null)
            {
                SellingShopUIManager.Instance.CloseMainShop();
            }
        }
    }
}
