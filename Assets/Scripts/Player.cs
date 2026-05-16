using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private TileManager tileManager;
    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tileManager != null)
            {
                // Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
                Vector3Int position =
    tileManager.interactableMap.WorldToCell(transform.position);
                string tileName  = tileManager.GetTileName(position);
                Debug.Log(tileName);
                Debug.Log(inventoryManager.toolbar.selectedSlot.itemName);
                if (!string.IsNullOrWhiteSpace(tileName))
                {
                    // Đào đất
                    if(tileName == "Interactable" && inventoryManager.toolbar.selectedSlot.itemName == "Hoe")
                    {
                        tileManager.SetInteracted(position);
                    }

                    // GIEO HẠT
                    else if (tileName == "Summer_Plowed")
                    {
                        ItemData selectedItem =
                        inventoryManager.toolbar.selectedSlot.itemData;

                        if (selectedItem != null &&
                            selectedItem.cropData != null)
                        {
                            Debug.Log(selectedItem.itemName);
                            Debug.Log(selectedItem.cropData);

                            tileManager.PlantCrop(position, selectedItem.cropData);

                            inventoryManager.toolbar.selectedSlot.RemoveItem();

                            GameManager.instance.uiManager.RefreshAll();
                        }
                    }

                    else if (inventoryManager.toolbar.selectedSlot.itemName == "Scythe")
                    {
                        Collider2D hit =
                            Physics2D.OverlapCircle(
                                transform.position,
                                1f);

                        if (hit != null)
                        {
                            Crop[] crops = FindObjectsByType<Crop>(
    FindObjectsSortMode.None);

                            foreach (Crop crop in crops)
                            {
                                if (crop.tilePosition == position)
                                {
                                    if (crop.CanHarvest())
                                    {
                                        crop.Harvest();

                                        tileManager.ResetPlowed(position);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        Vector2 spawmLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Item droppedItem = Instantiate(item, spawmLocation + spawnOffset, Quaternion.identity);
        droppedItem.transform.SetParent(null);
        droppedItem.transform.localScale = Vector3.one;

        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
