using UnityEngine;

[CreateAssetMenu(fileName = "New Building Data", menuName = "Building System/Building Data")]
public class BuildingData : ScriptableObject
{
    public enum PlacementType { GroundLevel, UpperLevel, Either }

    public string buildingName;
    public GameObject prefab;
    public PlacementType placementType;
}
