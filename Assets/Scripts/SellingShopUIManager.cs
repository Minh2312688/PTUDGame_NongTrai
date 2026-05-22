using UnityEngine;

public class SellingShopUIManager : MonoBehaviour
{
    // Tạo cơ chế Singleton giúp các script khác (như Script va chạm) gọi nhanh tới đây
    public static SellingShopUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainShopPanel; // Kéo thả MainShopPanel vào đây

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null)
        {
            Instance = this;
            // ĐỪNG để trong Start(), hãy ép ẩn UI ngay tại Awake để chạy trước toàn bộ va chạm vật lý
            ForceCloseOnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm chuyên dụng để ẩn UI tuyệt đối khi vừa load game
    private void ForceCloseOnAwake()
    {
        if (mainShopPanel != null)
        {
            mainShopPanel.SetActive(false);
        }
    }

    // Hàm mở cửa hàng (Sẽ được gọi khi Player va chạm)
    public void OpenMainShop()
    {
        if (mainShopPanel != null)
        {
            mainShopPanel.SetActive(true);
        }
    }

    // Hàm đóng cửa hàng (Sẽ được gọi khi Player đi ra xa hoặc bấm nút X)
    public void CloseMainShop()
    {
        if (mainShopPanel != null)
        {
            mainShopPanel.SetActive(false);
        }
    }
}