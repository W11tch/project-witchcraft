// Located at: Assets/Scripts/BuildingSystem/PlaceableObject.cs
using UnityEngine;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core; // Added for PlaceableItemData

namespace ProjectWitchcraft.BuildingSystem
{
    public class PlaceableObject : MonoBehaviour
    {
        [SerializeField] private PlaceableItemData itemData; // Changed from BuildingData
        public Vector3Int Size { get; private set; }
        public bool Placed { get; set; }
        public string UniqueID { get; set; }

        public PlaceableItemData ItemData => itemData; // Changed from BuildingData

        private void Awake()
        {
            Collider objectCollider = GetComponent<Collider>();
            if (objectCollider != null)
            {
                Vector3 size = objectCollider.bounds.size;
                Size = new Vector3Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), Mathf.Max(1, Mathf.RoundToInt(size.z)));
            }
        }

        public void Rotate()
        {
            transform.Rotate(0, 90, 0);
            Size = new Vector3Int(Size.z, Size.y, Size.x);
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
}