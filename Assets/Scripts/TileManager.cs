using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;
    void Start()
    {
        foreach(var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile= interactableMap.GetTile(pos);

           interactableMap.SetTile(pos,hiddenInteractableTile);
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null)
        {
            if (tile.name == "Interactable")
            { 
                return true;
            }
        }
        return false;
    }

    public void SetInteractable(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }

}
