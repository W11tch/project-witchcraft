using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private BuildingData buildingData;
    public Vector3Int Size { get; private set; }
    public bool Placed { get; set; }
    public string UniqueID { get; set; }

    public BuildingData BuildingData => buildingData;

    private void Awake()
    {
        Collider objectCollider = GetComponent<Collider>();
        if (objectCollider != null)
        {
            Vector3 size = objectCollider.bounds.size;
            Size = new Vector3Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), 1);
        }
    }

    public void Rotate()
    {
        transform.Rotate(0, 90, 0);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public Vector3 GetStartPosition()
    {
        return transform.position;
    }
}