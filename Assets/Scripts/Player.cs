using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(21);
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawmLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Collectable droppedItem = Instantiate(item, spawmLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }
}
