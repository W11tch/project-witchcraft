using UnityEngine;
using ProjectWitchcraft.BuildingSystem;

namespace ProjectWitchcraft.BuildingSystem
{
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
                // I've kept the improved version of this line as it's more robust and shouldn't cause issues.
                Size = new Vector3Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), Mathf.Max(1, Mathf.RoundToInt(size.z)));
            }
        }

        public void Rotate()
        {
            transform.Rotate(0, 90, 0);
            // This line is also a necessary bug fix for rotation to work correctly.
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