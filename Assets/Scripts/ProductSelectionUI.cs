using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ProductSelectionUI : MonoBehaviour
{
    [Header("Cấu hình Prefab")]
    [SerializeField] private GameObject itemSlotPrefab; // Kéo thả thanh Prefab SelectionItemSlot vào đây
    [SerializeField] private Transform scrollContent;   // Kéo ô Content của InventoryScroll vào đây

    [Header("Nhóm Điều Chỉnh (AdjustmentPanel)")]
    [SerializeField] private CanvasGroup adjustmentCanvasGroup; // Gắn Canvas Group của panel phải vào đây để làm mờ/khóa
    [SerializeField] private Button btnMinusAmount;
    [SerializeField] private Button btnPlusAmount;
    [SerializeField] private TextMeshProUGUI txtAmountDisplay;

    [SerializeField] private Button btnMinusPrice;
    [SerializeField] private Button btnPlusPrice;
    [SerializeField] private TextMeshProUGUI txtPriceDisplay;
    [SerializeField] private TextMeshProUGUI txtTotalGoldDisplay;
    [SerializeField] private Button btnConfirmSell;

    [Header("Dữ liệu Tính toán nội bộ")]
    private string selectedItemName = "";
    private int currentSelectAmount = 1;
    private int currentSelectPrice = 10; // Giá khởi điểm mặc định
    private int maxAllowedInInventory = 0;

    private void OnEnable()
    {
        // Mỗi khi Panel này được Active lên (bấm từ ô trắng ShopSlot)
        ResetSelectionUI();
        LoadAndCombineInventory();
    }

    // YÊU CẦU 1: Reset trạng thái ban đầu, khóa tương tác bên phải khi chưa chọn đồ
    private void ResetSelectionUI()
    {
        selectedItemName = "";
        currentSelectAmount = 1;
        currentSelectPrice = 10;
        maxAllowedInInventory = 0;

        UpdateAmountAndPriceTexts();

        // Khóa không cho click panel bên phải, làm mờ đi
        if (adjustmentCanvasGroup != null)
        {
            adjustmentCanvasGroup.alpha = 0.5f; // Làm mờ 50%
            adjustmentCanvasGroup.interactable = false; // Không cho tương tác nút
            adjustmentCanvasGroup.blocksRaycasts = false; // Chuột bấm xuyên qua không phản hồi
        }
    }

    // YÊU CẦU 2: Gộp sản phẩm từ cả Backpack và Toolbar hiện lên lưới ở giữa
    private void LoadAndCombineInventory()
    {
        // Xóa sạch các icon cũ trong danh sách cuộn tránh bị trùng lặp
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        if (InventoryManager.Instance == null) return;

        // Dùng Dictionary để cộng dồn số lượng các vật phẩm trùng tên từ 2 kho đồ riêng biệt
        Dictionary<string, (Sprite icon, int count)> combinedItems = new Dictionary<string, (Sprite, int)>();

        // 1. Quét kho đồ Backpack
        if (InventoryManager.Instance.backpack != null)
        {
            foreach (var slot in InventoryManager.Instance.backpack.slots)
            {
                if (!slot.IsEmpty)
                {
                    if (combinedItems.ContainsKey(slot.itemName))
                    {
                        var data = combinedItems[slot.itemName];
                        data.count += slot.count;
                        combinedItems[slot.itemName] = data;
                    }
                    else
                    {
                        combinedItems.Add(slot.itemName, (slot.icon, slot.count));
                    }
                }
            }
        }

        // 2. Quét tiếp thanh công cụ Toolbar dưới đáy màn hình
        if (InventoryManager.Instance.toolbar != null)
        {
            foreach (var slot in InventoryManager.Instance.toolbar.slots)
            {
                if (!slot.IsEmpty)
                {
                    if (combinedItems.ContainsKey(slot.itemName))
                    {
                        var data = combinedItems[slot.itemName];
                        data.count += slot.count;
                        combinedItems[slot.itemName] = data;
                    }
                    else
                    {
                        combinedItems.Add(slot.itemName, (slot.icon, slot.count));
                    }
                }
            }
        }

        // 3. Tiến hành Instantiation sinh ra giao diện trên lưới Content
        foreach (var item in combinedItems)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, scrollContent);
            SelectionItemSlot slotScript = newSlot.GetComponent<SelectionItemSlot>();
            if (slotScript != null)
            {
                // Cài đặt thông tin hiển thị và truyền hàm OnSlotClicked khi người chơi bấm chọn vật phẩm này
                slotScript.Setup(item.Key, item.Value.icon, item.Value.count, OnProductItemClicked);
            }
        }
    }

    // YÊU CẦU 3: Kích hoạt phát sáng rực rỡ bảng bên phải SAU KHI chọn 1 món hàng
    private void OnProductItemClicked(SelectionItemSlot clickedSlot)
    {
        selectedItemName = clickedSlot.ItemName;
        maxAllowedInInventory = clickedSlot.TotalOwnedCount; // Tổng số lượng tối đa thu thập được từ cả 2 kho
        currentSelectAmount = 1; // Reset số lượng về 1 khi đổi lựa chọn món hàng

        // Bật sáng phát sáng và cho phép click tương tác panel bên phải
        if (adjustmentCanvasGroup != null)
        {
            adjustmentCanvasGroup.alpha = 1.0f; // Sáng 100% rực rỡ
            adjustmentCanvasGroup.interactable = true; // Kích hoạt nút bấm nhấp nháy
            adjustmentCanvasGroup.blocksRaycasts = true; // Nhận diện chuột bình thường
        }

        UpdateAmountAndPriceTexts();
    }

    // YÊU CẦU 4: Logic tính toán cộng trừ số lượng không vượt quá Max = Inventory + Toolbar
    public void ClickIncreaseAmount()
    {
        if (currentSelectAmount < maxAllowedInInventory)
        {
            currentSelectAmount++;
            UpdateAmountAndPriceTexts();
        }
    }

    public void ClickDecreaseAmount()
    {
        if (currentSelectAmount > 1)
        {
            currentSelectAmount--;
            UpdateAmountAndPriceTexts();
        }
    }

    public void ClickIncreasePrice()
    {
        currentSelectPrice += 5; // Mỗi lần bấm tăng thêm 5 xu (Tùy em chỉnh)
        UpdateAmountAndPriceTexts();
    }

    public void ClickDecreasePrice()
    {
        if (currentSelectPrice > 1)
        {
            currentSelectPrice -= 5;
            if (currentSelectPrice < 1) currentSelectPrice = 1;
            UpdateAmountAndPriceTexts();
        }
    }

    // Cập nhật thông số tính toán lên màn hình chữ Text
    private void UpdateAmountAndPriceTexts()
    {
        if (txtAmountDisplay != null) txtAmountDisplay.text = currentSelectAmount.ToString();
        if (txtPriceDisplay != null) txtPriceDisplay.text = currentSelectPrice.ToString() + " Gold/món";

        // Tính tổng tiền = Số lượng x đơn giá từng món
        int totalGold = currentSelectAmount * currentSelectPrice;
        if (txtTotalGoldDisplay != null) txtTotalGoldDisplay.text = "Tổng: " + totalGold.ToString() + " Gold";
    }

    // Sự kiện khi bấm nút BÀY BÁN chốt đơn
    public void ClickConfirmSellButton()
    {
        Debug.Log($"[HỆ THỐNG SHOP] Đã chốt đem bán: {currentSelectAmount} món {selectedItemName} với tổng giá trị {currentSelectAmount * currentSelectPrice} xu!");

        // Đoạn này sau này anh em mình viết tiếp logic trừ đồ thật trong kho và ném lên ô của MainShop sau nhé!
        if (SellingShopUIManager.Instance != null)
        {
            SellingShopUIManager.Instance.CloseProductSelection();
        }
    }
}