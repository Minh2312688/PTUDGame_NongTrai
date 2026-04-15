using Newtonsoft.Json;
using UnityEngine;

public enum TilemapState
{
    Ground,
    Grass,
    Forest
}

public class TilemapDetails
{
    public int x {get;set;}
    public int y {get;set;}
    public TilemapState tilemapState {get;set;}

    public TilemapDetails()
    {
        
    }

    public TilemapDetails(int x, int y, TilemapState tilemapState)
    {
        this.x = x;
        this.y = y;
        this.tilemapState = tilemapState;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

}
