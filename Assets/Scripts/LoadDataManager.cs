using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;

public class LoadDataManager : MonoBehaviour
{
    public static FirebaseUser firebaseUser;
    public static User userInGame;
    private DatabaseReference reference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firebaseUser=FirebaseAuth.DefaultInstance.CurrentUser;
        FirebaseApp app=FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        GetUserInGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void GetUserInGame()
    {
        reference.Child("Users").Child("User/"+firebaseUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
    {
        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                userInGame=JsonConvert.DeserializeObject<User>(snapshot.Value.ToString());
               
            }
            else
            {
                Debug.Log("Không có dữ liệu");
            }
            return;
        }
        Debug.LogError("Đọc thất bại: " + task.Exception);
    });
    }
}
