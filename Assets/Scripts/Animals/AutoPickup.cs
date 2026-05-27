using UnityEngine;

public class AutoPickup : MonoBehaviour
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // chỉ player mới nhặt
        if(other.CompareTag("Player"))
        {
            // tìm inventory manager
            InventoryManager inventoryManager =
                FindAnyObjectByType<InventoryManager>();

            if(inventoryManager == null)
            {
                Debug.LogError(
                    "InventoryManager NULL");

                return;
            }

            // item null
            if(item == null)
            {
                Debug.LogError(
                    "Item component NULL");

                return;
            }

            // THÊM ITEM
            inventoryManager.Add(
                "Backpack",
                item);

            Debug.Log(
                "Picked Up: " +
                item.data.itemName);

            // XÓA ITEM
            Destroy(gameObject);
        }
    }
}