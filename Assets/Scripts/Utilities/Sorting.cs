using UnityEngine;
using UnityEngine.Serialization;

namespace Utilities
{
    public class Sorting : MonoBehaviour
    {
        [FormerlySerializedAs("SortingOrder")] public int sortingOrder = 0;
        
        void Start()
        {
            gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        }

    
    }
}
