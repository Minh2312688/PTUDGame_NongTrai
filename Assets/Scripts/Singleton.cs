using UnityEngine;

/// <summary>
/// Generic Singleton Pattern - Dùng cho tất cả Manager classes
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    singletonObject.name = typeof(T).Name;
                    instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        T component = GetComponent<T>();
        if (component != null)
        {
            instance = component;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public static bool HasInstance()
    {
        return instance != null;
    }

    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}
