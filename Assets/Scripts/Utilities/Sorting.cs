using UnityEngine;

namespace Utilities
{
    public class Sorting : MonoBehaviour
    {
        public int sortingOrder;
        
        void Start()
        {
            gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        }

    
    }
}
