using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

public class User
{
    public string Name{get;set;}
    public int Gold{get;set;}
    public int Diamond {get;set;}
    public Map MapInGame{get;set;}
    public User()
    {
    }
    public User(string Name,int Gold,int Diamond,Map MapInGame)
    {
         this.Name=Name;
         this.Gold=Gold;
         this.Diamond=Diamond;
         this.MapInGame=MapInGame;
        
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
