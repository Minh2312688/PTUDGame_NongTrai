using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int maxAllowed;

        public Sprite icon;

        public ItemData itemData;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool IsEmpty
        {
            get
            {
                if (itemName == "" && count == 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CanAddItem(string itemName)
        {
            if (this.itemName == itemName && count < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
            this.itemData = item.data;

            count++;
        }

        public void AddItem(string itemName,
                    Sprite icon,
                    int maxAllowed,
                    ItemData itemData)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.itemData = itemData;

            count++;
            this.maxAllowed = maxAllowed;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    itemName = "";
                    icon = null;
                    itemData = null;
                }
            }
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Slot selectedSlot = null;


    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
            //slots.Add(new Slot { type = CollectableType.NONE, count = 0, maxAllowed = 99 });
        }
    }

    public void Add(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public void Remove(int index, int numToRemove)
    {
        if (slots[index].count >= numToRemove)
        {
            for (int i = 0; i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove  = 1)
    {
        if (slots != null && slots.Count > 0)
        {
            Slot fromSlot = slots[fromIndex];
            Slot toSlot = toInventory.slots[toIndex];

            for (int i = 0; i < numToMove; i++)
            {
                if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName))
                {
                    toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed, fromSlot.itemData);
                    fromSlot.RemoveItem();
                }

            }
        }
       
    }

    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }
}

