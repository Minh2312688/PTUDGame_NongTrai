using UnityEngine;
using System.Collections;

public class SheepEat : Animal
{
    // Prefab lông
    public GameObject woolPrefab;

    // Đếm số lần sinh
    private int spawnCount = 0;

    void Start()
    {
        StartCoroutine(SheepLife());
    }

    IEnumerator SheepLife()
    {
        // Lần đầu tự sinh
        yield return new WaitForSeconds(9f);

        SpawnWool();

        // Các lần sau phải ăn
        while(spawnCount < 3)
        {
            Debug.Log("Waiting For Food");

            // Chờ cho ăn
            yield return new WaitUntil(() => isFed);

            Debug.Log("Sheep Eating");

            // Chờ tiêu hóa
            yield return new WaitForSeconds(9f);

            SpawnWool();

            // Reset
            isFed = false;
        }

        Die();
    }

    void SpawnWool()
    {
        // Vị trí sinh
        Vector3 spawnPos =
            transform.position + Vector3.right;

        // Sinh lông
        Instantiate(
            woolPrefab,
            spawnPos,
            Quaternion.identity);

        // Tăng số lần sinh
        spawnCount++;

        Debug.Log(
            "Spawn Count: " + spawnCount);
    }

    void Die()
    {
        Debug.Log("Sheep Dead");

        Destroy(gameObject);
    }
}