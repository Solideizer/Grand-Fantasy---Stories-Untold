using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onMouseOverScript : MonoBehaviour
{
    private Vector3 scaleChange;
    
    void OnMouseOver()
    {
        
        scaleChange = new Vector3(1.2f, 1.2f, 0f);
        transform.localScale = scaleChange;
    }

    void OnMouseExit()
    {
        scaleChange = new Vector3(1f, 1f, 0f);
        transform.localScale = scaleChange;
        
    }
}
