using UnityEngine;
public class OnMouseOverScript : MonoBehaviour
{
    private Vector3 _scaleChange;
    
    void OnMouseOver()
    {        
        _scaleChange = new Vector3(1.2f, 1.2f, 0f);
        transform.localScale = _scaleChange;
    }

    void OnMouseExit()
    {
        _scaleChange = new Vector3(1f, 1f, 0f);
        transform.localScale = _scaleChange;
        
    }
}
