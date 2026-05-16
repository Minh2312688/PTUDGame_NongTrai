using UnityEngine;

public class FeedZone : MonoBehaviour
{
    // Động vật
    public Animal animal;

    private bool playerNear = false;

    void Update()
    {
        // Đứng gần + bấm E
        if(playerNear &&
            Input.GetKeyDown(KeyCode.E))
        {
            animal.isFed = true;

            Debug.Log("Animal Fed");
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = true;

            Debug.Log("Player Near Feed Zone");
        }
    }

    private void OnTriggerExit2D(
        Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = false;

            Debug.Log("Player Left Feed Zone");
        }
    }
}