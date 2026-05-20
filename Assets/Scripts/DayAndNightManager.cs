using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayAndNightManager : MonoBehaviour
{
    public Text clockText;

    public float dayMultiplier = 20f;
    public Light2D light2D;
    public Gradient gradient;
    float saveTimer = 0f;
    FirebaseUser user;
    FireBaseDatabaseManager databaseManager;
    DatabaseReference reference;

    private int currentDay = 1;
    private int lastHour = 0;

    void Awake()
    {
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();
        user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            Debug.LogError("User chưa đăng nhập!");
            return;
        }

        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        LoadTimeInGameForUser();
    }

    void Update()
    {
        DateTime realtime = DateTime.Now;

        float realSecondsInday = realtime.Hour * 3600 + realtime.Minute * 60 + realtime.Second;
        realSecondsInday = (realSecondsInday * dayMultiplier) % 86400;

        int gameHours = Mathf.FloorToInt(realSecondsInday / 3600);
        if (lastHour == 23 && gameHours == 0)
        {
            currentDay++;
            Debug.Log("🌞 New Day: " + currentDay);
        }

        lastHour = gameHours;

        int gameMinutes = Mathf.FloorToInt((realSecondsInday % 3600) / 60);

        string timeFormatted = string.Format("{0:00}:{1:00}", gameHours, gameMinutes);

        if (clockText != null)
        {
            clockText.text = timeFormatted;
        }

        ChangeColorByTime(realSecondsInday);
         // ⏱️ lưu mỗi 5 giây
    saveTimer += Time.deltaTime;
    if (saveTimer >= 5f)
    {
        WriteTimeInGameToFirebase();
        saveTimer = 0f;
    }
    }

    void ChangeColorByTime(float seconds)
    {
        if (light2D != null)
            light2D.color = gradient.Evaluate(seconds / 86400);
    }

    public void LoadTimeInGameForUser()
    {
        reference.Child("TimeInGame").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted) return;

            DataSnapshot snapshot = task.Result;
            Debug.Log("snapshot: " + snapshot.Value.ToString());
            if (snapshot.Value != null)
                clockText.text = snapshot.Value.ToString();
        });
    }

    public void WriteTimeInGameToFirebase()
    {
        if (user != null)
            databaseManager.WriteDatabase("TimeInGame/" + user.UserId, clockText.text);
    }

    // 🕐 Lấy giờ hiện tại trong game (0-23)
    public int GetCurrentHour()
    {
        DateTime realtime = DateTime.Now;
        float realSecondsInday = realtime.Hour * 3600 + realtime.Minute * 60 + realtime.Second;
        realSecondsInday = (realSecondsInday * dayMultiplier) % 86400;
        return Mathf.FloorToInt(realSecondsInday / 3600);
    }

    // 🕐 Lấy thời gian hiện tại dạng string (HH:MM)
    public string GetCurrentTimeString()
    {
        return clockText.text;
    }

    // 📅 Lấy ngày hiện tại trong game
    public int GetCurrentDay()
    {
        DateTime realtime = DateTime.Now;

        float realSecondsInday =
            realtime.Hour * 3600 +
            realtime.Minute * 60 +
            realtime.Second;

        float totalGameDays =
            (realSecondsInday * dayMultiplier) / 86400f;

        return Mathf.FloorToInt(totalGameDays);
    }
}