using UnityEngine;
using TMPro;
public class CropTooltipUI : MonoBehaviour
{
    public static CropTooltipUI instance;

    public GameObject panel;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    private void Update()
    {
        panel.transform.position =
            Input.mousePosition + new Vector3(15, -15, 0);
    }

    public void Show(string info)
    {
        panel.SetActive(true);
        infoText.text = info;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
