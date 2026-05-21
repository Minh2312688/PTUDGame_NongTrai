using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip farmMusic;
    public AudioClip clickSound;
    public AudioClip plantHarvestSound;
    public AudioClip digSound;
    public AudioClip bagSound;
    public AudioClip walkSound;
    public AudioClip plantGrowSound;

    [Header("SFX Cooldown")]
    public float digCooldown = 0.3f;
    public float plantGrowCooldown = 0.3f;
    public float plantHarvestCooldown = 0.3f;
    public float bagCooldown = 0.2f;
    public float clickCooldown = 0.1f;
    public float walkCooldown = 0.4f;
    public float moneyCooldown = 0.1f;

    [Header("SFX Max Duration (seconds)")]
    public float digMaxDuration = 0.5f;
    public float plantGrowMaxDuration = 0.8f;
    public float plantHarvestMaxDuration = 1.0f;
    public float bagMaxDuration = 0.6f;
    public float clickMaxDuration = 0.3f;
    public float walkMaxDuration = 0.6f;
    public float moneyMaxDuration = 0.4f;

    private float lastDigTime = -1f;
    private float lastPlantGrowTime = -1f;
    private float lastPlantHarvestTime = -1f;
    private float lastBagTime = -1f;
    private float lastClickTime = -1f;
    private float lastWalkTime = -1f;
    private float lastMoneyTime = -1f;

    void Start()
    {
        PlayMusic(farmMusic);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
    }
    // 🎵 Play nhạc nền
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayDig()
    {
        if (Time.time - lastDigTime >= digCooldown)
        {
            PlaySFX(digSound);
            StartCoroutine(StopSFXAfterDuration(digMaxDuration));
            lastDigTime = Time.time;
        }
    }
    public void PlayPlantGrow()
    {
        if (Time.time - lastPlantGrowTime >= plantGrowCooldown)
        {
            PlaySFX(plantGrowSound);
            StartCoroutine(StopSFXAfterDuration(plantGrowMaxDuration));
            lastPlantGrowTime = Time.time;
        }
    }
    public void PlayPlantHarvest()
    {
        if (Time.time - lastPlantHarvestTime >= plantHarvestCooldown)
        {
            PlaySFX(plantHarvestSound);
            StartCoroutine(StopSFXAfterDuration(plantHarvestMaxDuration));
            lastPlantHarvestTime = Time.time;
        }
    }
    public void PlayBag()
    {
        if (Time.time - lastBagTime >= bagCooldown)
        {
            PlaySFX(bagSound);
            StartCoroutine(StopSFXAfterDuration(bagMaxDuration));
            lastBagTime = Time.time;
        }
    }
    public void PlayMoney(AudioClip clip)
    {
        if (Time.time - lastMoneyTime >= moneyCooldown)
        {
            PlaySFX(clip);
            StartCoroutine(StopSFXAfterDuration(moneyMaxDuration));
            lastMoneyTime = Time.time;
        }
    }
    public void PlayClick()
    {
        if (Time.time - lastClickTime >= clickCooldown)
        {
            PlaySFX(clickSound);
            StartCoroutine(StopSFXAfterDuration(clickMaxDuration));
            lastClickTime = Time.time;
        }
    }
    public void PlayWalk()
    {
        if (Time.time - lastWalkTime >= walkCooldown)
        {
            PlaySFX(walkSound);
            StartCoroutine(StopSFXAfterDuration(walkMaxDuration));
            lastWalkTime = Time.time;
        }
    }   
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // 🔊 Play sound effect
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Stop SFX after duration
    private System.Collections.IEnumerator StopSFXAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }

    // 🎚️ Volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
