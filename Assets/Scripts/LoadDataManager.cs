using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataManager : MonoBehaviour
{
    public static FirebaseUser firebaseUser;
    public static User userInGame;
    public Text userName;
    public Text diamond;
    public Text gold;
    public static bool IsUserLoaded { get; private set; }
    public static event Action<User> OnUserLoaded;
    private DatabaseReference reference;


    void Awake()
    {
        userInGame = new User();
        IsUserLoaded = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        GetUserInGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetUserInGame()
    {
        reference.Child("Users").Child("User/" + firebaseUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Đọc thất bại: " + task.Exception);
                userInGame = userInGame ?? new User();
                
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    userInGame = JsonConvert.DeserializeObject<User>(snapshot.Value.ToString());
                    userName.text = userInGame.Name;
                diamond.text = userInGame.Diamond.ToString();
                gold.text = userInGame.Gold.ToString();
                }
                else
                {
                    Debug.Log("Không có dữ liệu");
                    userInGame = new User();
                }
            }

            IsUserLoaded = true;
            OnUserLoaded?.Invoke(userInGame);
        });
    }
}
