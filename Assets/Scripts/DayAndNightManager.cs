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
    public static DayAndNightManager Instance { get; private set; }
    
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
    private DateTime loadedTime = DateTime.Now;
    private DateTime startTime = DateTime.Now;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        databaseManager = FireBaseDatabaseManager.Instance;
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
        // Tính thời gian đã trôi qua từ startTime
        DateTime currentRealtime = DateTime.Now;
        TimeSpan elapsedTime = currentRealtime - startTime;
        float realSecondsInday = (float)elapsedTime.TotalSeconds;
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
        WriteDayNightToFirebase();
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
            {
                string timeString = snapshot.Value.ToString();
                clockText.text = timeString;

                // Parse thời gian từ Firebase (HH:MM)
                string[] timeParts = timeString.Split(':');
                if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hours) && int.TryParse(timeParts[1], out int minutes))
                {
                    // Tính số giây trong ngày từ thời gian load
                    float savedSecondsInDay = hours * 3600 + minutes * 60;
                    
                    // Lưu base time (thời gian được load)
                    loadedTime = DateTime.Now;
                    startTime = DateTime.Now.AddSeconds(-savedSecondsInDay / dayMultiplier);

                    Debug.Log("✅ Load time from Firebase: " + timeString + " -> Recalculate from: " + startTime);
                }
            }
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
        DateTime currentRealtime = DateTime.Now;
        TimeSpan elapsedTime = currentRealtime - startTime;
        float realSecondsInday = (float)elapsedTime.TotalSeconds;
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
        DateTime currentRealtime = DateTime.Now;
        TimeSpan elapsedTime = currentRealtime - startTime;
        float realSecondsInday = (float)elapsedTime.TotalSeconds;

        float totalGameDays =
            (realSecondsInday * dayMultiplier) / 86400f;

        return Mathf.FloorToInt(totalGameDays) + 1;
    }

    // 🌞/🌙 Kiểm tra hiện tại là ngày hay đêm
    public bool IsDay()
    {
        int currentHour = GetCurrentHour();
        return currentHour >= 6 && currentHour < 18;
    }

    // 🌙 Kiểm tra hiện tại là đêm
    public bool IsNight()
    {
        return !IsDay();
    }

    // 📝 Lấy trạng thái ngày/đêm dạng string
    public string GetDayNightStatus()
    {
        if (IsDay())
            return "Day";
        else
            return "Night";
    }

    // 📤 Lưu ngày/đêm lên Firebase
    public void WriteDayNightToFirebase()
    {
        if (user != null)
        {
            string dayNightData = GetCurrentDay() + "_" + GetDayNightStatus();
            databaseManager.WriteDatabase("DayNight/" + user.UserId, dayNightData);
        }
    }
}