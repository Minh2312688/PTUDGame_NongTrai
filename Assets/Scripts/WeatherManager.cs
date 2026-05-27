using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public enum WeatherState { Sunny, Rainy, Overcast }

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }
    
    [Header("Weather Objects")]
    public GameObject rainEffect;

    [Header("Lighting")]
    public Light2D globalLight;

    [Header("Rain Settings")]
    public AudioClip rainSound;

    [Header("Weather Time")]
    public float weatherChangeTime = 30f;
    [Header("Weather Images")]

    public UnityEngine.UI.Image imgWeather;
    public Sprite sun;
    public Sprite rain;
    public Sprite moon;
    public Sprite overcast;

    private WeatherState currentWeather = WeatherState.Sunny;
    private DayAndNightManager dayNightManager;
    private float saveTimer = 0f;
    private FirebaseUser user;
    private FireBaseDatabaseManager databaseManager;
    private DatabaseReference reference;

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

        // Tìm DayAndNightManager trong Awake
        dayNightManager = FindAnyObjectByType<DayAndNightManager>();
        if (dayNightManager == null)
            Debug.LogError("Không tìm thấy DayAndNightManager!");
    }

    void Start()
    {
        // Load thời tiết từ Firebase
        LoadWeatherForUser();

        

        // tự đổi thời tiết
        InvokeRepeating(nameof(RandomWeather), weatherChangeTime, weatherChangeTime);
    }

    void Update()
    {
        // ⏱️ lưu mỗi 5 giây
        saveTimer += Time.deltaTime;
        if (saveTimer >= 5f)
        {
            WriteWeatherToFirebase();
            saveTimer = 0f;
        }

        // 🎮 Phím tắt để test thời tiết nhanh
        HandleQuickWeatherSwitching();
    }

    // Xử lý chuyển đổi nhanh thời tiết bằng phím tắt
    void HandleQuickWeatherSwitching()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ForceSunny();
            Debug.Log("✅ [TEST] Chuyển sang thời tiết NẮNG");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ForceRain();
            Debug.Log("✅ [TEST] Chuyển sang thời tiết MƯA");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ForceOvercast();
            Debug.Log("✅ [TEST] Chuyển sang thời tiết ÂM U");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RandomWeather();
            Debug.Log("✅ [TEST] Thời tiết NGẪU NHIÊN");
        }
        #endif
    }

    // random thời tiết
    void RandomWeather()
    {
        float weatherChance = Random.value;
        
        if (weatherChance > 0.65f)
        {
            SetSunny();

        }
        else if (weatherChance > 0.35f)
        {
            SetOvercast();
        }
        else
        {
            SetRain();
            
        }

        // Ghi thay đổi lên Firebase
        WriteWeatherToFirebase();
    }

    // 🌧️ trời mưa
    void SetRain()
    {
        Debug.Log("🌧️ Trời mưa");
        currentWeather = WeatherState.Rainy;

        // bật hiệu ứng mưa
        if (rainEffect != null)
            rainEffect.SetActive(true);

        // giảm sáng
        if (globalLight != null)
            globalLight.intensity = 0.6f;

        imgWeather.sprite = rain;

        // phát âm thanh mưa
        if (AudioManager.Instance != null && rainSound != null)
        {
            AudioManager.Instance.PlaySFX(rainSound);
        }
    }

    // 🌧️ trời âm u
    void SetOvercast()
    {
        Debug.Log("🌧️ Trời âm u");
        currentWeather = WeatherState.Overcast;

        // tắt hiệu ứng mưa
        if (rainEffect != null)
            rainEffect.SetActive(false);

        // giảm sáng vừa phải (giữa nắng và mưa)
        if (globalLight != null)
            globalLight.intensity = 0.8f;

        imgWeather.sprite = overcast;
    }

    // ☀️ trời nắng
    void SetSunny()
    {
        Debug.Log("☀️ Trời nắng");
        currentWeather = WeatherState.Sunny;

        // tắt mưa
        if (rainEffect != null)
            rainEffect.SetActive(false);

        // tăng sáng
        if (globalLight != null)
            globalLight.intensity = 1f;

        // Hiển thị sprite dựa trên ngày/đêm từ DayAndNightManager
        if (dayNightManager != null)
        {
            if (dayNightManager.IsDay())
            {
                imgWeather.sprite = sun;
            }
            else
            {
                imgWeather.sprite = moon;
            }
        }
        else
        {
            imgWeather.sprite = sun;
        }
    }

    // gọi thủ công từ button nếu muốn
    public void ForceRain()
    {
        SetRain();
        WriteWeatherToFirebase();
    }

    public void ForceSunny()
    {
        SetSunny();
        WriteWeatherToFirebase();
    }

    public void ForceOvercast()
    {
        SetOvercast();
        WriteWeatherToFirebase();
    }

    // 📥 Load thời tiết từ Firebase
    public void LoadWeatherForUser()
    {
        reference.Child("Weather").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("Lỗi load thời tiết, sử dụng mặc định");
                return;
            }

            DataSnapshot snapshot = task.Result;
            Debug.Log("Load thời tiết: " + snapshot.Value?.ToString());
            
            if (snapshot.Value != null)
            {
                string weatherType = snapshot.Value.ToString();
                
                if (weatherType == "Rainy")
                    SetRain();
                else if (weatherType == "Overcast")
                    SetOvercast();
                else
                    SetSunny();
            }
            else{
                SetSunny();
            }
        });
    }

    // 📤 Lưu thời tiết lên Firebase
    public void WriteWeatherToFirebase()
    {
        if (user != null)
        {
            databaseManager.WriteDatabase("Weather/" + user.UserId, currentWeather.ToString());
        }
    }

    public bool IsRaining()
    {
        return currentWeather == WeatherState.Rainy;
    }

    public bool IsOvercast()
    {
        return currentWeather == WeatherState.Overcast;
    }

    public WeatherState GetCurrentWeather()
    {
        return currentWeather;
    }
}
