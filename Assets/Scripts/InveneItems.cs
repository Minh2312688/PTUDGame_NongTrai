using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class InveneItems 
{
   public List<Invenitem> lstInventItem{get;set;}
    public InveneItems()
    {
        lstInventItem = new List<Invenitem>();
    }   
    public InveneItems(List<Invenitem> lstInventItem)
    {
        this.lstInventItem = lstInventItem;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
   
    public int GetLength()
    {
        return lstInventItem?.Count ?? 0;
    }
}
