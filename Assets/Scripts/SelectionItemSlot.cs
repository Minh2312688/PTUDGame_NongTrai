using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SelectionItemSlot : MonoBehaviour
{
    [Header("UI Components con")]
    public Image itemIconDisplay;
    public TextMeshProUGUI countTextDisplay;

    // Lưu trữ thông tin vật phẩm ẩn bên trong nút để khi Click truyền đi xử lý
    public string ItemName { get; private set; }
    public Sprite ItemSprite { get; private set; }
    public int TotalOwnedCount { get; private set; }

    private System.Action<SelectionItemSlot> onSlotSelectedCallback;

    public void Setup(string name, Sprite icon, int totalCount, System.Action<SelectionItemSlot> onSelectCallback)
    {
        ItemName = name;
        ItemSprite = icon;
        TotalOwnedCount = totalCount;
        onSlotSelectedCallback = onSelectCallback;

        if (itemIconDisplay != null) itemIconDisplay.sprite = icon;
        if (countTextDisplay != null) countTextDisplay.text = "x" + totalCount;

        // Gắn sự kiện click chuột trực tiếp bằng code cho an toàn
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => onSlotSelectedCallback?.Invoke(this));
    }
}
