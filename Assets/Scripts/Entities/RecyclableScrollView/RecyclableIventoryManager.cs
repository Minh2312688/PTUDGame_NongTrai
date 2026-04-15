using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;

public class RecyclableInventoryManager : MonoBehaviour,IRecyclableScrollRectDataSource
{
        [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    [SerializeField]
    private int _dataLength;
    public GameObject inventoryGameObject;
    private List<Invenitems> _ivenItems = new List<Invenitems>();
    private void Awake()
    {
    _recyclableScrollRect.DataSource = this;
    }
    
    public int GetItemCount()
    {
    return _ivenItems.Count;
    }
   
    public void SetCell(ICell cell, int index)
    {
    //Casting to the implemented Cell
    var item = cell as CellItemData;
    item.ConfigureCell(_ivenItems[index],index);
    }
    void Start()
    {
        List<Invenitems> lstItems = new List<Invenitems>();
        for (int i = 0; i < 50; i++)
        {
            Invenitems invenItem = new Invenitems();
            invenItem.name = "Name_"+i.ToString();
            invenItem.Description  = "Description of item " + i;
            lstItems.Add(invenItem);
        }
        SetLstItems(lstItems);
        _recyclableScrollRect.ReloadData();
    }
    public void SetLstItems(List<Invenitems> lstItems)
    {
        _ivenItems = lstItems;
        _recyclableScrollRect.ReloadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Invenitems invenItemDemo = new Invenitems("Cá","Ca");
            _ivenItems.Add(invenItemDemo);
            _recyclableScrollRect.ReloadData();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            //inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);
        Vector3 crrPosInven=inventoryGameObject.GetComponent<RectTransform>().anchoredPosition;
        inventoryGameObject.GetComponent<RectTransform>().anchoredPosition= crrPosInven.y==1000?Vector3.zero:new Vector3(0,1000,0);

        }
    }
    public void addInventoryItem(Invenitems item)
    {
        _ivenItems.Add(item);
        _recyclableScrollRect.ReloadData();
    }
}
