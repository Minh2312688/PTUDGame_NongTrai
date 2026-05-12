using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;

    private Dictionary<CollectableType, Collectable> collectableItemsDict = new Dictionary<CollectableType, Collectable>();

    private void Awake()
    {
        foreach (Collectable item in collectableItems)
        {
            if (!collectableItemsDict.ContainsKey(item.type))
            {
                AddItem(item);
            }
        }
    }

    private void AddItem(Collectable item)
    {
        // This method can be used to add items to the dictionary if needed in the future
        if (!collectableItemsDict.ContainsKey(item.type))
        {
            collectableItemsDict.Add(item.type, item);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        if (collectableItemsDict.ContainsKey(type))
        {
            return collectableItemsDict[type];

        }
        else
        {
            Debug.LogWarning($"Item of type {type} not found in ItemManager.");
            return null;
        }
    }
}
