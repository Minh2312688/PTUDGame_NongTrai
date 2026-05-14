using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "ScriptableObjects/ItemData", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
}
