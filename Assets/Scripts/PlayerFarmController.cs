using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum TileState
{
    Ground,
    Grass,
    Forest
}

public class PlayerFarmController : MonoBehaviour
{
    public Tilemap tm_Ground; // tilemap chứa các ô đất
    public Tilemap tm_Grass;
    public Tilemap tm_Forest;
    public TileBase tb_Ground;
    public TileBase tb_Grass;
    public TileBase tb_Forest;
    public TileMapManager tileMapManager;
    RecyclableInventoryManager recyclableInventoryManager;

    void Update()
    {
        HandleFarmAction();
    }
    void Start()
    {
        recyclableInventoryManager = RecyclableInventoryManager.Instance;
    }
    public void HandleFarmAction()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            AudioManager.Instance.PlayDig();

            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("Cell Position: " + cellPos);
            TileBase currentTile = tm_Grass.GetTile(cellPos);
            if (currentTile == tb_Grass) 
            {
                tm_Grass.SetTile(cellPos, null);// xóa ô cỏ
                tileMapManager.SetStateTilemapDetail(cellPos.x,cellPos.y,TilemapState.Ground);
            }
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            AudioManager.Instance.PlayPlantGrow();
            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("Cell Position: " + cellPos);
            TileBase currentTile = tm_Grass.GetTile(cellPos);
            if (currentTile == null)
            {
                tm_Forest.SetTile(cellPos, tb_Forest);// trồng cây      
                tileMapManager.SetStateTilemapDetail(cellPos.x,cellPos.y,TilemapState.Forest);
   
                
            }
        }
        //Thu hoạch cây
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.PlayPlantHarvest();

            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("Cell Position: " + cellPos);
            TileBase currentTile = tm_Forest.GetTile(cellPos);
            if (currentTile !=null)
            {
                tm_Forest.SetTile(cellPos, null);// chặt cây
                tm_Grass.SetTile(cellPos, tb_Grass);// trồng cỏ
                //Lay item va them vao tui do
                Invenitem itemFlower = new Invenitem();
                itemFlower.name = "Hoa 1h";
                itemFlower.Description="Hoa này trang trí rất đẹp";
                Debug.Log("Thu hoạch được: " + itemFlower.name);
                recyclableInventoryManager.addInventoryItem(itemFlower);
            }
        }
    }

}
