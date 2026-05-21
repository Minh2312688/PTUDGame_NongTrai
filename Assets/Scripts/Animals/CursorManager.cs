using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }
    
    public Texture2D baoCursor;

    public Texture2D buaCursor;

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

    // Tay mở
    public void SetBao()
    {
        Cursor.SetCursor(
            baoCursor,
            new Vector2(32, 32),
            CursorMode.Auto);
    }

    // Tay nắm
    public void SetBua()
    {
        Cursor.SetCursor(
            buaCursor,
            new Vector2(32, 32),
            CursorMode.Auto);
    }

    // Trở về chuột thường
    public void ResetCursor()
    {
        Cursor.SetCursor(
            null,
            Vector2.zero,
            CursorMode.Auto);
    }

    // Animation nhặt
    public IEnumerator PickupAnimation()
    {
        SetBua();

        yield return new WaitForSeconds(0.15f);

        ResetCursor();
    }
}