using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
public class RecyclableInventoryManager : MonoBehaviour, IRecyclableScrollRectDataSource
{
    public static RecyclableInventoryManager Instance { get; private set; }
    
        [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    [SerializeField]
    private int _dataLength;
    public GameObject inventoryGameObject;
    InveneItems invenItems;
    FirebaseUser user;
    DatabaseReference reference;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _recyclableScrollRect.DataSource = this;

        user = FirebaseAuth.DefaultInstance.CurrentUser;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public int GetItemCount()
    {
    return invenItems.lstInventItem.Count;
    }
   
    public void SetCell(ICell cell, int index)
    {
    //Casting to the implemented Cell
    var item = cell as CellItemData;
    item.ConfigureCell(invenItems.lstInventItem[index],index);
    }
    void Start()
    {
        invenItems = new InveneItems();
        LoadInventory(); // 🔥 load từ firebase

    if (invenItems.lstInventItem.Count == 0)
    {
        List<Invenitem> lstItems = new List<Invenitem>();

        for (int i = 0; i < 20; i++)
        {
            Invenitem invenItem = new Invenitem();
            invenItem.name = "Name_" + i;
            invenItem.Description = "Description of item " + i;
            lstItems.Add(invenItem);
        }
        SaveInventory(); // 🔥 save lên firebase
        SetLstItems(lstItems);
    }

    _recyclableScrollRect.ReloadData();
    }
    public void SetLstItems(List<Invenitem> lstItems)
    {
        invenItems = new InveneItems(lstItems);
        _recyclableScrollRect.ReloadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Invenitem invenItemDemo = new Invenitem("Cá","Ca");
            invenItems.lstInventItem.Add(invenItemDemo);
            _recyclableScrollRect.ReloadData();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
        AudioManager.Instance.PlayBag();

            //inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);
        Vector3 crrPosInven=inventoryGameObject.GetComponent<RectTransform>().anchoredPosition;
        inventoryGameObject.GetComponent<RectTransform>().anchoredPosition= crrPosInven.y==1000?Vector3.zero:new Vector3(0,1000,0);

        }
    }
    public void addInventoryItem(Invenitem item)
{
    Invenitem existingItem = invenItems.lstInventItem.Find(x => x.name == item.name);

    if (existingItem != null)
    {
        existingItem.quantity++;
    }
    else
    {
        invenItems.lstInventItem.Add(item);

    }

    _recyclableScrollRect.ReloadData();
    SaveInventory(); // 🔥 auto save
}
    public void removeInventoryItem(Invenitem item)
{
    Invenitem existingItem = invenItems.lstInventItem.Find(x => x.name == item.name);

    if (existingItem != null)
    {
        if (existingItem.quantity > 1)
        {
            existingItem.quantity--;
        }
        else
        {
            invenItems.lstInventItem.Remove(existingItem);
        }

        _recyclableScrollRect.ReloadData();
        SaveInventory(); // 🔥 auto save
    }
}
    public void SaveInventory()
{
    if (user == null) return;

    
        reference.Child("Users")
            .Child("Inventory")
            .Child(user.UserId)
            .SetValueAsync(invenItems.ToString());
    
}
public void LoadInventory()
{
    if (user == null) return;

    reference.Child("Users")
        .Child("Inventory")
        .Child(user.UserId)
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
    {
        if (task.IsCanceled || task.IsFaulted) return;
            invenItems.lstInventItem.Clear();
            DataSnapshot snapshot=task.Result;
            Debug.Log("snapshot: " + snapshot.Value.ToString());
            invenItems.lstInventItem=JsonConvert.DeserializeObject<InveneItems>(snapshot.Value.ToString()).lstInventItem;
            Debug.Log("load inventory: " + invenItems.ToString());
        _recyclableScrollRect.ReloadData();
    });
}
}
