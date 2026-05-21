# Singleton Pattern Implementation Guide

## Overview
Singleton pattern đã được áp dụng cho toàn bộ hệ thống Manager để đảm bảo chỉ có một instance của mỗi Manager tồn tại.

## Base Singleton Class
Tất cả Manager đều inherit từ `Singleton<T>` generic class:
```csharp
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
```

**File:** `Assets/Scripts/Singleton.cs`

### Features:
- Tự động tạo instance nếu chưa tồn tại
- DontDestroyOnLoad: Preserve across scenes
- `Instance` property để truy cập singleton
- `HasInstance()`: Kiểm tra xem instance có tồn tại không
- `DestroyInstance()`: Destroy singleton instance

## Managers Converted to Singleton

### Core Managers:
1. **GameManager** - Main game controller
   - Access: `GameManager.Instance`
   - Contains: ItemManager, TileManager, UI_Manager, Player reference

2. **FireBaseDatabaseManager** - Database operations
   - Access: `FireBaseDatabaseManager.Instance`
   - Replace: `GameObject.Find("DatabaseManager")` → `FireBaseDatabaseManager.Instance`

3. **FirebaseLoginManager** - Authentication
   - Access: `FirebaseLoginManager.Instance`

4. **LoadDataManager** - User data loading
   - Access: `LoadDataManager.Instance`
   - Static events: `OnUserLoaded`

### Game System Managers:
5. **WeatherManager** - Weather system
   - Access: `WeatherManager.Instance`
   - States: Sunny, Rainy, Overcast

6. **DayAndNightManager** - Day/Night cycle
   - Access: `DayAndNightManager.Instance`

7. **UIManager** - Main UI system
   - Access: `UIManager.Instance`

8. **UI_Manager** - Inventory UI
   - Access: `UI_Manager.Instance`

### Inventory & Item Managers:
9. **InventoryManager** - Inventory management
   - Access: `InventoryManager.Instance`

10. **ItemManager** - Item database
    - Access: `ItemManager.Instance`

11. **RecyclableInventoryManager** - Scrollable inventory
    - Access: `RecyclableInventoryManager.Instance`

### Tile & Interaction Managers:
12. **TileMapManager** - Tilemap operations
    - Access: `TileMapManager.Instance`

13. **TileManager** - Tile interactions
    - Access: `TileManager.Instance`

### Audio & Input Managers:
14. **AudioManager** - Sound effects & music
    - Access: `AudioManager.Instance`

15. **CursorManager** - Cursor management
    - Access: `CursorManager.Instance`

16. **CropTooltipUI** - Crop tooltips
    - Access: `CropTooltipUI.Instance`

## Migration Guide

### Old Pattern:
```csharp
// Method 1: Static variable
public static GameManager instance;

void Awake()
{
    if (instance != null && instance != this)
        Destroy(gameObject);
    else
        instance = this;
    DontDestroyOnLoad(gameObject);
}

// Access
GameManager.instance.SomeMethod();
```

### New Pattern:
```csharp
// Inherit from Singleton<T>
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();  // Call base first
        // Your initialization here
    }
}

// Access
GameManager.Instance.SomeMethod();
```

## Key Changes Made:

1. **Replaced all `GameObject.Find()` calls:**
   ```csharp
   // Old
   databaseManager = GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();
   
   // New
   databaseManager = FireBaseDatabaseManager.Instance;
   ```

2. **Fixed case sensitivity:**
   ```csharp
   // Old: GameManager.instance (lowercase i)
   // New: GameManager.Instance (uppercase I)
   ```

3. **Updated instance properties:**
   ```csharp
   // Old: public static ClassName instance;
   // New: Automatic via Singleton<T>.Instance property
   ```

## Benefits:

✅ **Consistency:** Uniform singleton pattern across all managers  
✅ **Safety:** Thread-safe instance creation  
✅ **Cleanliness:** No manual singleton boilerplate  
✅ **Flexibility:** Easy to extend with `HasInstance()` and `DestroyInstance()`  
✅ **Performance:** Removes GameObject.Find() overhead  
✅ **Maintainability:** Single source of truth for singleton logic  

## Usage Examples:

```csharp
// Access managers
GameManager gameManager = GameManager.Instance;
InventoryManager inventory = InventoryManager.Instance;
AudioManager audio = AudioManager.Instance;
WeatherManager weather = WeatherManager.Instance;

// Check if instance exists
if (AudioManager.HasInstance())
{
    AudioManager.Instance.PlaySFX(clip);
}

// Destroy instance if needed
AudioManager.DestroyInstance();
```

## Notes:

- All managers now use `protected override void Awake()` instead of `void Awake()`
- Must call `base.Awake()` first in overridden Awake methods
- The `Awake()` method is called before any `Start()` methods
- Instance is automatically created on first access if not already instantiated
