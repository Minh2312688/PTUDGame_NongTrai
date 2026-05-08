using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    public Image imgDiamond;
    public Text txtDiamond;
    public Image imgGold;
    public Text txtGold;
    public Image imgWatch;
    public Text txtWatch;
    public Button btnSettings;
    public Button btnLogout;
    public Button btnHide;
    public Sprite iconView;
    public Sprite iconHide;
    private bool isHidden = false;
     void Start()
    {
        btnHide.onClick.AddListener(HideUI);
        btnLogout.onClick.AddListener(FirebaseLoginManager.SignOut);
    }
    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
    public void HideUI()
    {

         isHidden = !isHidden; // đảo trạng thái

        // đổi icon
        btnHide.GetComponent<Image>().sprite = isHidden ? iconView : iconHide;       
        imgDiamond.gameObject.SetActive(!imgDiamond.gameObject.activeSelf);
        txtDiamond.gameObject.SetActive(!txtDiamond.gameObject.activeSelf);
        imgGold.gameObject.SetActive(!imgGold.gameObject.activeSelf);
        txtGold.gameObject.SetActive(!txtGold.gameObject.activeSelf);
        imgWatch.gameObject.SetActive(!imgWatch.gameObject.activeSelf);
        txtWatch.gameObject.SetActive(!txtWatch.gameObject.activeSelf);
        btnLogout.gameObject.SetActive(!btnLogout.gameObject.activeSelf);
        btnSettings.gameObject.SetActive(!btnSettings.gameObject.activeSelf);
    }
}
