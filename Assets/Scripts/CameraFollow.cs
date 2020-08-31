using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector2 offset = new Vector2(5,0);
    public Transform playerTransform;   
    
    void LateUpdate()
    {
        Vector3 temp = transform.position;

        temp.x = playerTransform.position.x + offset.x;

        transform.position = temp;
    }
}
