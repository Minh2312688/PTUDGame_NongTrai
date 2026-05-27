using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PigEat : Animal
{
    // Prefab thịt
    public GameObject meatPrefab;

    // Đếm số lần sinh
    private int spawnCount = 0;

    void Start()
    {
        // ensure slider exists
        if (eatSlider == null)
        {
            eatSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
            if (eatSlider == null)
                CreateEatSlider();
        }

        if (eatSlider != null)
            eatSlider.gameObject.SetActive(false);

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

        public Slider eatSlider;
        public float eatTime = 5f;

        private bool isEating = false;

        public void StartEating()
        {
            if (isEating) return;

            isFed = true;
            StartCoroutine(EatingProcess());
        }

        IEnumerator EatingProcess()
        {
            isEating = true;

            if (eatSlider != null)
            {
                eatSlider.gameObject.SetActive(true);
                eatSlider.maxValue = eatTime;
                eatSlider.value = 0f;
            }

            float timer = 0f;
            while (timer < eatTime)
            {
                timer += Time.deltaTime;
                if (eatSlider != null)
                    eatSlider.value = timer;
                yield return null;
            }

            if (eatSlider != null)
                eatSlider.gameObject.SetActive(false);

            SpawnMeat();

            isFed = false;
            isEating = false;
        }

    void CreateEatSlider()
    {
        GameObject canvasObj = new GameObject("EatSlider_Canvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = Vector3.up * 1.2f;
        var canvas = canvasObj.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
        var rect = canvasObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1.5f, 0.2f);

        GameObject sliderObj = new GameObject("EatSlider");
        sliderObj.transform.SetParent(canvasObj.transform, false);
        var slider = sliderObj.AddComponent<UnityEngine.UI.Slider>();
        slider.minValue = 0;
        slider.maxValue = eatTime;
        slider.value = 0;
        sliderObj.AddComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0);

        RectTransform sRect = sliderObj.GetComponent<RectTransform>();
        sRect.sizeDelta = new Vector2(150, 20);

        eatSlider = slider;
    }

    public bool IsEating()
    {
        return isEating;
    }
}