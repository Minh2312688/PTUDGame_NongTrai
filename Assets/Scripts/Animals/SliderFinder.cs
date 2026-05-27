using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tự động tìm Slider từ child objects và gắn vào component
/// Giúp tránh phải gắn thủ công trên Inspector
/// </summary>
public class SliderFinder : MonoBehaviour
{
    private void OnEnable()
    {
        // Tìm Slider từ child
        Slider foundSlider = GetComponentInChildren<Slider>();
        
        if (foundSlider == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Không tìm thấy Slider trong child objects. " +
                "Vui lòng tạo UI Canvas với Slider hoặc gắn thủ công.", gameObject);
            return;
        }

        // Gắn Slider cho ChickenEat
        ChickenEat chickenEat = GetComponent<ChickenEat>();
        if (chickenEat != null)
        {
            chickenEat.eatSlider = foundSlider;
            Debug.Log($"[{gameObject.name}] Tìm thấy Slider cho ChickenEat", gameObject);
            return;
        }

        // Gắn Slider cho CowEat
        CowEat cowEat = GetComponent<CowEat>();
        if (cowEat != null)
        {
            cowEat.eatSlider = foundSlider;
            Debug.Log($"[{gameObject.name}] Tìm thấy Slider cho CowEat", gameObject);
            return;
        }

        // Gắn Slider cho SheepEat
        SheepEat sheepEat = GetComponent<SheepEat>();
        if (sheepEat != null)
        {
            sheepEat.eatSlider = foundSlider;
            Debug.Log($"[{gameObject.name}] Tìm thấy Slider cho SheepEat", gameObject);
            return;
        }

        // Gắn Slider cho PigEat
        PigEat pigEat = GetComponent<PigEat>();
        if (pigEat != null)
        {
            pigEat.eatSlider = foundSlider;
            Debug.Log($"[{gameObject.name}] Tìm thấy Slider cho PigEat", gameObject);
            return;
        }
    }
}
