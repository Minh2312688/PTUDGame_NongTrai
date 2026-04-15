using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager:MonoBehaviour
{
    public Tilemap tm_Ground;
    public Tilemap tm_Grass;
    public Tilemap tm_Forest;
    public TileBase tb_Forest;
    Map map;
    FireBaseDatabaseManager databaseManager;
    FirebaseUser user;
    DatabaseReference reference;
    
    void Start()
    {
        map=new Map();
        databaseManager=GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();
        user=FirebaseAuth.DefaultInstance.CurrentUser;
        //WriteAllTileMapToFirebase();
        FirebaseApp app=FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        LoadMapForUser();
    }
    void WriteAllTileMapToFirebase()
    {
        List<TilemapDetails> tilemaps = new List<TilemapDetails>();
        for (int x = tm_Ground.cellBounds.min.x; x < tm_Ground.cellBounds.max.x; x++)
        {
            for (int y = tm_Ground.cellBounds.min.y; y < tm_Ground.cellBounds.max.y; y++)
            {
                TilemapDetails tm_detail=new TilemapDetails(x,y,TilemapState.Grass);
                tilemaps.Add(tm_detail); 
            }

        }
        map.lstTilemapDetail=tilemaps;
        Debug.Log(map.ToString());
        databaseManager.WriteDatabase(user.UserId+"/Map" ,map.ToString());
    }
    public void LoadMapForUser()
    {
       reference.Child("Users").Child(user.UserId+"/Map").GetValueAsync().ContinueWithOnMainThread(task =>
    {
        if(task.IsCanceled)return;
        else if(task.IsFaulted)return;
        else if(task.IsCompleted)
        {
            DataSnapshot snapshot=task.Result;
            Debug.Log("snapshot: " + snapshot.Value.ToString());
            map=JsonConvert.DeserializeObject<Map>(snapshot.Value.ToString());

        }
         Debug.Log("load map: " + map.ToString());
         MapToUI(map);

    });
    }
    public void TilemapDetailToTileBase(TilemapDetails tilemapDetail)
    {
       Vector3Int cellPos=new Vector3Int(tilemapDetail.x,tilemapDetail.y,0);
       if(tilemapDetail.tilemapState==TilemapState.Ground)
        {
            tm_Grass.SetTile(cellPos,null);
            tm_Forest.SetTile(cellPos,null);
        }
        else if(tilemapDetail.tilemapState==TilemapState.Grass)
        {
            tm_Forest.SetTile(cellPos,null);
        }
        else if(tilemapDetail.tilemapState==TilemapState.Forest)
        {
            tm_Grass.SetTile(cellPos,null);
            tm_Forest.SetTile(cellPos,tb_Forest);
        }
    }
    public void MapToUI(Map map)
    {
       for(int i=0;i<map.GetLength();i++)
       {
           TilemapDetailToTileBase(map.lstTilemapDetail[i]);

       }
    }
    public void SetStateTilemapDetail(int x,int y,TilemapState state)
    {
                        Debug.Log("Vao 1");

        for(int i=0;i<map.GetLength();i++)
        {
                                    Debug.Log("Vao 2");

            if(map.lstTilemapDetail[i].x==x && map.lstTilemapDetail[i].y==y)
            {
                map.lstTilemapDetail[i].tilemapState=state;
                Debug.Log("Save to firebase succesful");
                databaseManager.WriteDatabase(user.UserId+"/Map" ,map.ToString());
                Debug.Log(map.ToString());

                break;

            }
        }

    }

}
