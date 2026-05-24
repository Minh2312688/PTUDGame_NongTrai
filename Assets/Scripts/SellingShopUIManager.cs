using UnityEngine;

public class SellingShopUIManager : MonoBehaviour
{
    // Tạo cơ chế Singleton giúp các script khác (như Script va chạm) gọi nhanh tới đây
    public static SellingShopUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainShopPanel; // Kéo thả MainShopPanel vào đây
    [SerializeField] private GameObject productSelectionPanel; // Khung chọn vật phẩm để bán

    // Biến lưu trữ vị trí ô vừa được bấm (Để sau này biết chọn hàng cho ô nào)
    public int CurrentSelectedSlotIndex { get; private set; } = -1;

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null)
        {
            Instance = this;
            // ĐỪNG để trong Start(), hãy ép ẩn UI ngay tại Awake để chạy trước toàn bộ va chạm vật lý
            ForceCloseOnAwake();
            ForceCloseOnAwakeProductSelectionPanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm chuyên dụng để ẩn UI cho màn hình chọn cửa hàng MainShopPanel tuyệt đối khi vừa load game
    private void ForceCloseOnAwake()
    {
        if (mainShopPanel != null)
        {
            mainShopPanel.SetActive(false);
        }
    }

    // Hàm chuyên dụng để ẩn UI cho màn hình chọn sản phẩm ProductSelectionPanel tuyệt đối khi vừa load game
    private void ForceCloseOnAwakeProductSelectionPanel()
    {
        if (productSelectionPanel != null)
        {
            productSelectionPanel.SetActive(false);
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

    // --- QUẢN LÝ PRODUCT SELECTION PANEL ---
    // Hàm này sẽ được gọi khi bấm vào 1 ô trống bất kỳ trong 15 ô
    public void OpenProductSelection(int slotIndex)
    {
        CurrentSelectedSlotIndex = slotIndex; // Ghi nhớ lại vừa bấm vào ô thứ mấy
        if (productSelectionPanel != null)
        {
            productSelectionPanel.SetActive(true);
        }
        Debug.Log("Đang mở bảng chọn hàng cho ô hiển thị số: " + slotIndex);
    }

    public void CloseProductSelection()
    {
        if (productSelectionPanel != null)
        {
            productSelectionPanel.SetActive(false);
        }
        CurrentSelectedSlotIndex = -1; // Reset vị trí chọn
    }

}