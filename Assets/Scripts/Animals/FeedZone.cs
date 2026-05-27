using System.Collections.Generic;
using UnityEngine;

public class FeedZone : MonoBehaviour
{
    [Header("Feed Setup")]
    public string feedItemName = "Feed Chicken";

    private bool playerNear = false;

    private InventoryManager inventoryManager;
    private Collider2D feedCollider;
    private ContactFilter2D animalContactFilter;

    private void Start()
    {
        inventoryManager =
            FindObjectOfType<InventoryManager>();
        feedCollider = GetComponent<Collider2D>();

        animalContactFilter = new ContactFilter2D();
        animalContactFilter.useTriggers = true;
        animalContactFilter.useLayerMask = false;
    }

    void Update()
    {
        // đứng gần + nhấn SPACE
        if(playerNear &&
            Input.GetKeyDown(KeyCode.Space))
        {
            FeedAnimalsInZone();
        }
    }

    void FeedAnimalsInZone()
    {
        Inventory toolbar =
            inventoryManager.GetInventoryByName(
                "Toolbar");

        if(toolbar == null)
            return;
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        if (feedCollider != null)
        {
            feedCollider.Overlap(animalContactFilter, overlappingColliders);
        }

        List<Animal> animals = new List<Animal>();
        foreach (var collider in overlappingColliders)
        {
            if (collider == null)
                continue;

            Animal zoneAnimal = collider.GetComponent<Animal>();
            if (zoneAnimal != null && !animals.Contains(zoneAnimal))
            {
                animals.Add(zoneAnimal);
            }
        }

        if (animals.Count == 0)
        {
            Debug.Log("Không có động vật trong chuồng để cho ăn.");
            return;
        }

        // Tìm item feed tương ứng với từng loài động vật
        foreach (var animal in animals)
        {
            string requiredFeedItem = GetFeedItemNameForAnimal(animal);
            if (string.IsNullOrEmpty(requiredFeedItem))
                continue;

            int feedSlotIndex = FindFeedSlotIndex(toolbar, requiredFeedItem);
            if (feedSlotIndex < 0)
            {
                Debug.Log($"Không có {requiredFeedItem} cho {animal.GetType().Name}");
                continue;
            }

            // Trừ thức ăn
            toolbar.Remove(feedSlotIndex);

            // Cho ăn
            FeedAnimalInstance(animal);
        }
    }

    string GetFeedItemNameForAnimal(Animal animal)
    {
        // Tất cả động vật dùng chung item feed "Feed Chicken"
        return "Feed Chicken";
    }

    int FindFeedSlotIndex(Inventory toolbar, string feedItemName)
    {
        for (int i = 0; i < toolbar.slots.Count; i++)
        {
            Inventory.Slot slot = toolbar.slots[i];
            if (slot.itemName == feedItemName && slot.count > 0)
            {
                return i;
            }
        }

        return -1;
    }

    void FeedAnimalInstance(Animal animal)
    {
        if (animal == null)
            return;

        if (animal.TryGetComponent<ChickenEat>(out ChickenEat chicken))
        {
            chicken.StartEating();
            return;
        }

        if (animal.TryGetComponent<CowEat>(out CowEat cow))
        {
            cow.StartEating();
            return;
        }

        if (animal.TryGetComponent<SheepEat>(out SheepEat sheep))
        {
            sheep.StartEating();
            return;
        }

        if (animal.TryGetComponent<PigEat>(out PigEat pig))
        {
            pig.StartEating();
            return;
        }

        animal.isFed = true;
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