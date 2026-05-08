using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLoginManager:MonoBehaviour
{
    //Đăng ký
    [Header("Register")]
    public InputField ipRegisterEmail;
    public InputField ipRegisterPassword;
    public Button btnRegister;
    //Đăng nhập
    [Header("Login")]
    public InputField ipLoginEmail;
    public InputField ipLoginPassword;
    public Button btnLogin;
    // Firebase authencantion -->đăng ký, đăng nhập
    static FirebaseAuth auth;
    //Chuyển đổi qua lại giữa đăng ký và đăng nhập
    [Header("Switch form")]
    public Button btnMoveToLogin;
    public Button btnMoveToRegister;
    public GameObject registerForm;
    public GameObject loginForm;
    //Upload data usser
    private FireBaseDatabaseManager databaseManager;


    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        btnRegister.onClick.AddListener(RegisterAcountWithFirebase);
        btnLogin.onClick.AddListener(SigInWithFirebase);
        btnMoveToLogin.onClick.AddListener(SwitchForm);
        btnMoveToRegister.onClick.AddListener(SwitchForm);
        databaseManager=GetComponent<FireBaseDatabaseManager>();
    }
    void RegisterAcountWithFirebase()
    {
        string email = ipRegisterEmail.text;
        string password = ipRegisterPassword.text;
        if(password.Length < 6)
        {
            Debug.Log("Password phải >= 6 ký tự");
        }
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread( task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Đăng ký bị hủy");
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Đăng ký bị thất bại: " + task.Exception);
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("Đăng ký thành công: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Map mapInGame = new Map();
            User userInGame = new User("", 100, 50, mapInGame);

            databaseManager.WriteDatabase("User/" + newUser.UserId, userInGame.ToString());
            //Chuyển màn chơi sau khi đăng nhập thành công
            SceneManager.LoadScene("PlayScene");
        });
    }
    public void SigInWithFirebase()
    {
        string email = ipLoginEmail.text;
        string password = ipLoginPassword.text;
        if(password.Length < 6)
        {
            Debug.Log("Password phải >= 6 ký tự");
        }
        _ = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(static task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Đăng nhập bị hủy");
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Đăng nhập bị thất bại: " + task.Exception);
            }

            FirebaseUser user = task.Result.User;
            Debug.LogFormat("Đăng nhập thành công: {0} ({1})",
                user.DisplayName, user.UserId);
            
            //Chuyển màn chơi sau khi đăng nhập thành công
            SceneManager.LoadScene("PlayScene");
        });
    }
    public static void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }
    public void SwitchForm ()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }
}
