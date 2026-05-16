using UnityEngine;
using System.Collections;

public class PigEat : Animal
{
    // Prefab thịt
    public GameObject meatPrefab;

    // Đếm số lần sinh
    private int spawnCount = 0;

    void Start()
    {
        StartCoroutine(PigLife());
    }

    IEnumerator PigLife()
    {
        // Lần đầu tự sinh
        yield return new WaitForSeconds(8f);

        SpawnMeat();

        // Các lần sau phải ăn
        while(spawnCount < 3)
        {
            Debug.Log("Waiting For Food");

            // Chờ cho ăn
            yield return new WaitUntil(() => isFed);

            Debug.Log("Pig Eating");

            // Chờ tiêu hóa
            yield return new WaitForSeconds(8f);

            SpawnMeat();

            // Reset
            isFed = false;
        }

        Die();
    }

    void SpawnMeat()
    {
        // Vị trí sinh
        Vector3 spawnPos =
            transform.position + Vector3.right;

        // Sinh thịt
        Instantiate(
            meatPrefab,
            spawnPos,
            Quaternion.identity);

        // Tăng số lần sinh
        spawnCount++;

        Debug.Log(
            "Spawn Count: " + spawnCount);
    }

    void Die()
    {
        Debug.Log("Pig Dead");

        Destroy(gameObject);
    }
}