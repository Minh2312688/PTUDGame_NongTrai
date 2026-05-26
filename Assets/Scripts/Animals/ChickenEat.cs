using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChickenEat : Animal
{
    public GameObject eggPrefab;

    public Slider eatSlider;

    public float eatTime = 3f;

    private int eggCount = 0;

    private bool isEating = false;

    void Start()
    {
        if(eatSlider != null)
        {
            eatSlider.gameObject.SetActive(false);
        }

        StartCoroutine(ChickenLife());
    }

    IEnumerator ChickenLife()
    {
        // lần đầu tự đẻ
        yield return new WaitForSeconds(5f);

        LayEgg();

        while(eggCount < 3)
        {
            // chờ cho ăn
            yield return new WaitUntil(
                () => isFed);

            // ăn
            yield return StartCoroutine(
                EatingProcess());

            // đẻ
            LayEgg();

            isFed = false;
        }

        Die();
    }

    IEnumerator EatingProcess()
    {
        isEating = true;

        if(eatSlider != null)
        {
            eatSlider.gameObject.SetActive(true);

            eatSlider.maxValue = eatTime;

            eatSlider.value = 0;
        }

        float timer = 0;

        while(timer < eatTime)
        {
            timer += Time.deltaTime;

            if(eatSlider != null)
            {
                eatSlider.value = timer;
            }

            yield return null;
        }

        if(eatSlider != null)
        {
            eatSlider.gameObject.SetActive(false);
        }

        isEating = false;
    }

    void LayEgg()
    {
        Instantiate(
            eggPrefab,
            transform.position,
            Quaternion.identity);

        eggCount++;

        Debug.Log(
            "Egg Count: " + eggCount);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public bool IsEating()
    {
        return isEating;
    }
}