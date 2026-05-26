using UnityEngine;

public class FeedZone : MonoBehaviour
{
    public Animal animal;

    private bool playerNear = false;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager =
            FindObjectOfType<InventoryManager>();
    }

    void Update()
    {
        // đứng gần + nhấn SPACE
        if(playerNear &&
            Input.GetKeyDown(KeyCode.Space))
        {
            FeedAnimal();
        }
    }

    void FeedAnimal()
    {
        Inventory toolbar =
            inventoryManager.GetInventoryByName(
                "Toolbar");

        if(toolbar == null)
            return;

        // tìm thức ăn
        for(int i = 0;
            i < toolbar.slots.Count;
            i++)
        {
            Inventory.Slot slot =
                toolbar.slots[i];

            // item tên Food
            if(slot.itemName == "Food" &&
                slot.count > 0)
            {
                // trừ thức ăn
                toolbar.Remove(i);

                // cho ăn
                animal.isFed = true;

                Debug.Log("Animal Fed");

                return;
            }
        }

        Debug.Log("No Food");
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = true;

            Debug.Log(
                "Player Near Feed Zone");
        }
    }

    private void OnTriggerExit2D(
        Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}