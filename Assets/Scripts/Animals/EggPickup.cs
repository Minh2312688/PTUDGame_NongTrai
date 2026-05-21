using UnityEngine;
using System.Collections;

public class EggPickup : MonoBehaviour
{
    private bool isMouseOver;

    private InventoryManager inventoryManager;

    private Item item;

    private void Start()
    {
        inventoryManager =
            FindObjectOfType<InventoryManager>();

        item = GetComponent<Item>();
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;

        CursorManager.Instance.SetBao();
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

        CursorManager.Instance.ResetCursor();
    }

    private void Update()
    {
        if (isMouseOver &&
            Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PickupEgg());
        }
    }

    IEnumerator PickupEgg()
    {
        // Tay nắm
        CursorManager.Instance.SetBua();

        yield return new WaitForSeconds(0.1f);

        // Thêm vào túi đồ
        inventoryManager.Add("Backpack", item);

        // Reset chuột
        CursorManager.Instance.ResetCursor();

        // Xóa item
        Destroy(gameObject);
    }
}