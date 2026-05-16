using UnityEngine;
using System.Collections;

public class CowEat : Animal
{
    // Prefab sữa
    public GameObject MilkPrefab;

    // Đếm số lần sinh
    private int spawnCount = 0;

    void Start()
    {
        StartCoroutine(CowLife());
    }

    IEnumerator CowLife()
    {
        // Lần đầu tự sinh
        yield return new WaitForSeconds(9f);

        SpawnMilk();

        // Các lần sau phải ăn
        while(spawnCount < 3)
        {
            Debug.Log("Waiting For Food");

            // Chờ cho ăn
            yield return new WaitUntil(() => isFed);

            Debug.Log("Cow Eating");

            // Chờ tiêu hóa
            yield return new WaitForSeconds(9f);

            SpawnMilk();

            // Reset
            isFed = false;
        }

        Die();
    }

    void SpawnMilk()
    {
        // Vị trí sinh
        Vector3 spawnPos =
            transform.position + Vector3.right;

        // Sinh sữa
        Instantiate(
            MilkPrefab,
            spawnPos,
            Quaternion.identity);

        // Tăng số lần sinh
        spawnCount++;

        Debug.Log(
            "Spawn Count: " + spawnCount);
    }

    void Die()
    {
        Debug.Log("Cow Dead");

        Destroy(gameObject);
    }
}