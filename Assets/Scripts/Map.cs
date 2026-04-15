using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Map 
{
    public List<TilemapDetails> lstTilemapDetail{get;set;}
    public Map()
    {
    }   
    public Map(List<TilemapDetails> lstTilemapDetail)
    {
        this.lstTilemapDetail = lstTilemapDetail;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
   
    public int GetLength()
    {
        return lstTilemapDetail.Count;
    }
    
}
