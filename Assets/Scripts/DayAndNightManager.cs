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
    private bool isTimeLoaded = false;
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
        if (!isTimeLoaded) return;
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
        reference.Child("Users").Child("TimeInGame").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            try
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("❌ Lỗi load thời gian từ Firebase: " + task.Exception);
                    InitializeDefaultTime();
                    return;
                }

                DataSnapshot snapshot = task.Result;
                Debug.Log("📊 Checking path: Users/TimeInGame/" + user.UserId);
                Debug.Log("snapshot.Exists: " + snapshot.Exists);
                Debug.Log("snapshot.Value: " + (snapshot.Value ?? "NULL"));
                
                // Nếu không tìm thấy ở "TimeInGame/{userId}", thử path khác
                if (!snapshot.Exists)
                {
                    Debug.LogWarning("⚠️ Không tìm thấy dữ liệu ở Users/TimeInGame/" + user.UserId);
                    Debug.LogWarning("🔍 Thử kiểm tra cấu trúc database...");
                    
                    // Thử lấy toàn bộ Users/TimeInGame để debug
                    reference.Child("Users").Child("TimeInGame").GetValueAsync().ContinueWithOnMainThread(debugTask =>
                    {
                        if (!debugTask.IsFaulted)
                        {
                            DataSnapshot debugSnapshot = debugTask.Result;
                            if (debugSnapshot.Exists)
                            {
                                Debug.Log("📋 Dữ liệu trong Users/TimeInGame:");
                                foreach (DataSnapshot child in debugSnapshot.Children)
                                {
                                    Debug.Log("  - " + child.Key + ": " + child.Value);
                                }
                            }
                            else
                            {
                                Debug.Log("Users/TimeInGame không tồn tại");
                            }
                        }
                    });
                    
                    InitializeDefaultTime();
                    return;
                }

                // Nếu tồn tại, tiến hành parse
                if (snapshot.Value != null)
                {
                    string timeString = snapshot.Value.ToString();
                    Debug.Log("📥 Raw data from Firebase: '" + timeString + "'");

                    // Kiểm tra clockText trước khi assign
                    if (clockText == null)
                    {
                        Debug.LogError("❌ clockText chưa được assign!");
                        InitializeDefaultTime();
                        return;
                    }

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
                        isTimeLoaded = true;
                    }
                    else
                    {
                        Debug.LogError("❌ Không thể parse thời gian: '" + timeString + "'. Expected format: HH:MM");
                        InitializeDefaultTime();
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ snapshot.Value = NULL");
                    InitializeDefaultTime();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Exception trong LoadTimeInGameForUser: " + ex.Message + "\n" + ex.StackTrace);
                InitializeDefaultTime();
            }
        });
    }

    private void InitializeDefaultTime()
    {
        loadedTime = DateTime.Now;
        startTime = DateTime.Now;
        clockText.text = "00:00";
        isTimeLoaded = true;
        Debug.Log("✅ Khởi tạo thời gian mặc định: 00:00");
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