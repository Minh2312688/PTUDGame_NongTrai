using UnityEngine;

public class SellingShopSlotButton : MonoBehaviour
{
    [Header("Cấu hình số thứ tự ô (0 đến 17)")]
    public int slotIndex;

    // Hàm này sẽ liên kết với sự kiện OnClick của chính cái Button này
    public void OnSlotClicked()
    {
        if (SellingShopUIManager.Instance != null)
        {
            // Ra lệnh mở bảng chọn sản phẩm và gửi kèm số vị trí của ô này
            SellingShopUIManager.Instance.OpenProductSelection(slotIndex);
        }
    }
}
