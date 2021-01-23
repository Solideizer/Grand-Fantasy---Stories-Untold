using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private LayoutElement layoutElement;
#pragma warning restore 0649
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        
        _rectTransform.pivot = new Vector2(pivotX,pivotY);
        transform.position = position;
    }

    public void SetText(string content,string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled =
            headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }

    
}
