using UnityEngine;
using UnityEngine.UI;

public class UsernameWizard : MonoBehaviour
{
    public Text username;
    public Text gold;
    public Text diamond;

    public GameObject usernameWizard;
    public InputField ipUsername;
    public Button buttonOK;
    FireBaseDatabaseManager databaseManager;

    public static bool IsUsernameWizardOpen { get; private set; }

    void OnEnable()
    {
        LoadDataManager.OnUserLoaded += HandleUserLoaded;
    }

    void OnDisable()
    {
        LoadDataManager.OnUserLoaded -= HandleUserLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        databaseManager = FireBaseDatabaseManager.Instance;
        SetUsernameWizardActive(false);

        // Nếu dữ liệu đã load, xử lý ngay; nếu chưa thì event sẽ xử lý khi load xong
        if (LoadDataManager.IsUserLoaded)
        {
            HandleUserLoaded(LoadDataManager.userInGame);
        }

        buttonOK.onClick.AddListener(SetNewUsername);
        ipUsername.onValueChanged.AddListener(OnUsernameValueChanged);
        ipUsername.onEndEdit.AddListener(OnUsernameEndEdit);
        buttonOK.interactable = !string.IsNullOrWhiteSpace(ipUsername.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewUsername()
    {
        if (!string.IsNullOrWhiteSpace(ipUsername.text))
        {
            LoadDataManager.userInGame.Name = ipUsername.text;
            databaseManager.WriteDatabase("User/" + LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());
            username.text = ipUsername.text;
            SetUsernameWizardActive(false);
        }
    }

    private void OnUsernameValueChanged(string value)
    {
        buttonOK.interactable = !string.IsNullOrWhiteSpace(value);
    }

    private void OnUsernameEndEdit(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            SetNewUsername();
        }
    }

    private void HandleUserLoaded(User user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            SetUsernameWizardActive(true);
        }
        else
        {
            username.text = user.Name;
            gold.text=user.Gold.ToString();
            diamond.text=user.Diamond.ToString();

            SetUsernameWizardActive(false);
        }
    }

    private void SetUsernameWizardActive(bool active)
    {
        usernameWizard.SetActive(active);
        IsUsernameWizardOpen = active;
        if (active)
        {
            ipUsername.text = string.Empty;
            buttonOK.interactable = false;
            ipUsername.ActivateInputField();
        }
    }
}
