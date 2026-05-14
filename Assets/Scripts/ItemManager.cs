using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();

    private void Awake()
    {
        foreach (Item item in items)
        {
            
                AddItem(item);
           
        }
    }

    private void AddItem(Item item)
    {
        // This method can be used to add items to the dictionary if needed in the future
        if (!nameToItemDict.ContainsKey(item.data.itemName))
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        if (nameToItemDict.ContainsKey(key))
        {
            return nameToItemDict[key];
        }
        else
        {
            Debug.LogWarning($"Item with name {key} not found in ItemManager.");
            return null;
        }
    }
}
