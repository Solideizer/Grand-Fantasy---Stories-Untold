using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorting : MonoBehaviour
{
    public int SortingOrder = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }

    
}
