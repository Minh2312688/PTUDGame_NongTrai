using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class Invenitem 
{
    public string name{get;set; }
    public Sprite icon;
    public string Description{get;set; }
    public int quantity{get;set; }
    public Invenitem(string name, string description)
    {
        this.name = name;
        this.Description = description;
        this.quantity = 1;
    }
    public Invenitem(string name, string description, Sprite icon)
    {
        this.name = name;
        this.Description = description;
        this.icon = icon;
        this.quantity = 1;
    }
    public Invenitem ()
    {
        
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

}
