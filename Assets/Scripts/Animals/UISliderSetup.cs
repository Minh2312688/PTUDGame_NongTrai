using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tự động tạo UI Canvas + Slider nếu chưa có
/// Gắn Slider vào component Eat script
/// </summary>
public class UISliderSetup : MonoBehaviour
{
    private void Start()
    {
        // Tìm Slider hiện có
        Slider existingSlider = GetComponentInChildren<Slider>();
        
        if (existingSlider != null)
        {
            // Đã có Slider, chỉ cần gắn vào component
            SetupSliderForEatComponent(existingSlider);
            return;
        }

        // Tạo Canvas nếu chưa có
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("UI Canvas");
            canvasObj.transform.SetParent(transform);
            canvasObj.transform.localPosition = Vector3.zero;
            
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            
            RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(200, 50);
            canvasRect.anchoredPosition = new Vector3(0, 1.5f, 0); // Trên đầu động vật
        }

        // Tạo Slider
        GameObject sliderObj = new GameObject("EatSlider");
        sliderObj.transform.SetParent(canvas.transform);
        sliderObj.transform.localPosition = Vector3.zero;

        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.sizeDelta = new Vector2(100, 20);

        Image sliderImage = sliderObj.AddComponent<Image>();
        sliderImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 5; // Giá trị mặc định, sẽ được set lại trong EatingProcess
        slider.value = 0;
        slider.direction = Slider.Direction.LeftToRight;

        // Tạo Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(sliderObj.transform);
        bgObj.transform.localPosition = Vector3.zero;

        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        slider.targetGraphic = bgImage;

        // Tạo Fill Area
        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(sliderObj.transform);
        fillAreaObj.transform.localPosition = Vector3.zero;

        RectTransform fillAreaRect = fillAreaObj.AddComponent<RectTransform>();
        fillAreaRect.offsetMin = new Vector2(5, 5);
        fillAreaRect.offsetMax = new Vector2(-5, -5);

        // Tạo Fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform);
        fillObj.transform.localPosition = Vector3.zero;

        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        if (fillRect == null) fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = new Color(0.2f, 0.8f, 0.2f, 0.8f); // Xanh lá

        slider.fillRect = fillRect;

        // Ẩn Slider khi không ăn
        sliderObj.SetActive(false);

        // Gắn Slider vào component
        SetupSliderForEatComponent(slider);

        Debug.Log($"[{gameObject.name}] Tạo UI Slider thành công", gameObject);
    }

    void SetupSliderForEatComponent(Slider slider)
    {
        // Gắn cho ChickenEat
        ChickenEat chickenEat = GetComponent<ChickenEat>();
        if (chickenEat != null)
        {
            chickenEat.eatSlider = slider;
            return;
        }

        // Gắn cho CowEat
        CowEat cowEat = GetComponent<CowEat>();
        if (cowEat != null)
        {
            cowEat.eatSlider = slider;
            return;
        }

        // Gắn cho SheepEat
        SheepEat sheepEat = GetComponent<SheepEat>();
        if (sheepEat != null)
        {
            sheepEat.eatSlider = slider;
            return;
        }

        // Gắn cho PigEat
        PigEat pigEat = GetComponent<PigEat>();
        if (pigEat != null)
        {
            pigEat.eatSlider = slider;
            return;
        }
    }
}
