using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager : MonoBehaviour
{
    public Tilemap interactableMap;
    public Tile hiddenInteractableTile;
    public Tile plowedTile;
    public Tile plantedTile;

    [Header("Crops")]
    public GameObject wheatCropPrefab;
    void Start()
    {
        foreach(var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile= interactableMap.GetTile(pos);
            if (tile != null && tile.name == "Interactable_Visible")
            { interactableMap.SetTile(pos, hiddenInteractableTile); }
           
        }
    }

    //public bool IsInteractable(Vector3Int position)
    //{
    //    TileBase tile = interactableMap.GetTile(position);

    //    if (tile != null)
    //    {
    //        if (tile.name == "interactable")
    //        { 
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public string GetTileName(Vector3Int position)
    {
        
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if(tile != null)
            {
                return tile.name;
            }
        }
        return "";
    }

    public void PlantCrop(Vector3Int position, CropData cropData)
    {
        Vector3 spawnPosition =
            interactableMap.GetCellCenterWorld(position);

        Instantiate(cropData.cropPrefab,
            spawnPosition,
            Quaternion.identity);

        interactableMap.SetTile(position, plantedTile);
    }

}
