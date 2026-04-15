using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class Invenitems 
{
    public string name{get;set; }
    public string Description{get;set; }
    public Invenitems(string name, string description)
    {
        this.name = name;
        this.Description = description;
    }
    public Invenitems ()
    {
        
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

}
