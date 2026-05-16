using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string seedName;

    public GameObject cropPrefab;

    public float growTime;
}
