using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FireBaseDatabaseManager : MonoBehaviour
{
    DatabaseReference reference;
    // Chạy trước khi game được khởi tạo (trước frae ảnh dầu tiên xuất hiện)
    void Awake()
    {
        FirebaseApp app=FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }
    void Start()
    {
        
    }
    public void WriteDatabase(string path,string message)
    {
        reference.Child("Users").Child(path).SetValueAsync(message)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Ghi dữ liệu thành công");
                return;
            }
            Debug.LogError("Ghi thất bại: " + task.Exception);
        });
    }
    public void ReadDatabase(string path)
    {
         reference.Child("Users").Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
    {
        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                Debug.Log("Doc du lieu thanh cong: " + snapshot.Value.ToString());
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
