using Microsoft.Unity.VisualStudio.Editor;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class WeatherManager : MonoBehaviour
{
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

    public bool isRaining = false;
    private DayAndNightManager dayNightManager;
    private float saveTimer = 0f;
    private FirebaseUser user;
    private FireBaseDatabaseManager databaseManager;
    private DatabaseReference reference;

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

        // Tìm DayAndNightManager trong Awake
        dayNightManager = FindObjectOfType<DayAndNightManager>();
        if (dayNightManager == null)
            Debug.LogError("Không tìm thấy DayAndNightManager!");
    }

    void Start()
    {
        // Load thời tiết từ Firebase
        LoadWeatherForUser();

        // bắt đầu trời nắng
        SetSunny();

        // tự đổi thời tiết
        InvokeRepeating(nameof(RandomWeather), 5f, weatherChangeTime);
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
    }

    // random thời tiết
    void RandomWeather()
    {
        isRaining = Random.value > 0.5f;

        if (isRaining)
        {
            SetRain();
        }
        else
        {
            SetSunny();
        }

        // Ghi thay đổi lên Firebase
        WriteWeatherToFirebase();
    }

    // 🌧️ trời mưa
    void SetRain()
    {
        Debug.Log("🌧️ Trời mưa");

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

    // ☀️ trời nắng
    void SetSunny()
    {
        Debug.Log("☀️ Trời nắng");

        // tắt mưa
        if (rainEffect != null)
            rainEffect.SetActive(false);

        // tăng sáng
        if (globalLight != null)
            globalLight.intensity = 1f;

        // Hiển thị sprite dựa trên giờ trong game
        if (dayNightManager != null)
        {
            int currentHour = dayNightManager.GetCurrentHour();
            if (currentHour >= 6 && currentHour < 18)
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
        isRaining = true;
        SetRain();
        WriteWeatherToFirebase();
    }

    public void ForceSunny()
    {
        isRaining = false;
        SetSunny();
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
                bool loadedRaining = (bool)snapshot.Value;
                isRaining = loadedRaining;
                
                if (isRaining)
                    SetRain();
                else
                    SetSunny();
            }
        });
    }

    // 📤 Lưu thời tiết lên Firebase
    public void WriteWeatherToFirebase()
    {
        if (user != null)
        {
            databaseManager.WriteDatabase("Weather/" + user.UserId, isRaining.ToString());
        }
    }

    public bool IsRaining()
    {
        return isRaining;
    }
}
