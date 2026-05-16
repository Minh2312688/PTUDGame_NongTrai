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

        GameObject cropObj =
    Instantiate(
        cropData.cropPrefab,
        spawnPosition,
        Quaternion.identity);

        Crop crop = cropObj.GetComponent<Crop>();

        crop.tilePosition = position;

        interactableMap.SetTile(position, plantedTile);
    }

    //public void SetPlowed(Vector3Int position)
    //{
    //    interactableMap.SetTile(position, plowedTile);
    //}

    public void ResetPlowed(Vector3Int position)
    {
        interactableMap.SetTile(
            position,
            hiddenInteractableTile);
    }

}
