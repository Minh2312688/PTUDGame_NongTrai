using UnityEngine;
using System.Collections;

public class ChickenEat : Animal
{
    public GameObject eggPrefab;

    private int eggCount = 0;

    
    
    void Start()
    {
        StartCoroutine(ChickenLife());
    }

    IEnumerator ChickenLife()
    {
        // Đẻ lần đầu tự động
        yield return new WaitForSeconds(5f);

        LayEgg();

        // Các lần sau phải ăn
        while(eggCount < 3)
        {
            Debug.Log("Waiting For Food");

            yield return new WaitUntil(() => isFed);

            Debug.Log("Chicken Eating");

            yield return new WaitForSeconds(5f);

            LayEgg();

            isFed = false;
        }

        Die();
    }

    void LayEgg()
    {
        Instantiate(
            eggPrefab,
            transform.position,
            Quaternion.identity);

        eggCount++;

        Debug.Log("Egg Count: " + eggCount);
    }

    void Die()
    {
        Debug.Log("Chicken Dead");

        Destroy(gameObject);
    }
}