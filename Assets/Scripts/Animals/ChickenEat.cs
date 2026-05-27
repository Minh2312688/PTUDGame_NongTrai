using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChickenEat : Animal
{
    public GameObject eggPrefab;

    public Slider eatSlider;

    public float eatTime = 5f;

    private int eggCount = 0;

    private bool isEating = false;

    void Start()
    {
        // ensure slider exists (try find or create)
        if (eatSlider == null)
        {
            eatSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
            if (eatSlider == null)
                CreateEatSlider();
        }

        if(eatSlider != null)
        {
            eatSlider.gameObject.SetActive(false);
        }

        // đẻ lần đầu tự động
        StartCoroutine(FirstEgg());
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

    IEnumerator FirstEgg()
    {
        yield return new WaitForSeconds(5f);

        LayEgg();
    }


  public void StartEating()
{
    if(isEating) return;

    Debug.Log("START EATING CALLED"); // 

    isFed = true;
    StartCoroutine(EatingProcess());
}

    IEnumerator EatingProcess()
    {
        isEating = true;

        Debug.Log("Chicken Eating");

        // hiện thanh
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

        // ẩn thanh
        if(eatSlider != null)
        {
            eatSlider.gameObject.SetActive(false);
        }

        // đẻ trứng
        LayEgg();

        // reset
        isFed = false;
        isEating = false;
        //StartEating();

        // chết sau 3 lần
        if(eggCount >= 3)
        {
            Die();
        }
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
        Debug.Log("Chicken Dead");

        Destroy(gameObject);
    }

    // script di chuyển dùng
    public bool IsEating()
    {
        return isEating;
    }
    
}