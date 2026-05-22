using UnityEngine;

public class SellingShopTrigger : MonoBehaviour
{
    // Hàm tự động chạy khi Player bước vào vùng gạch của cửa hàng
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTED");

            if (SellingShopUIManager.Instance != null)
            {
                Debug.Log("OPEN SHOP");

                SellingShopUIManager.Instance.OpenMainShop();
            }
        }
    }

    // Hàm tự động chạy khi Player đi ra khỏi vùng gạch của cửa hàng
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("EXIT");

        //if (collision.CompareTag("Player"))
        //{
        //    if (SellingShopUIManager.Instance != null)
        //    {
        //        SellingShopUIManager.Instance.CloseMainShop();
        //    }
        //}
    }
}
