using System.Collections;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager _instance;

    public Tooltip tooltip;

    private void Awake()
    {
        _instance = this;
    }

    public static void Hide()
    {
        _instance.tooltip.gameObject.SetActive(false);
    }

    public static IEnumerator ShowTooltip(string content,string header = "")
    {
        yield return new WaitForSeconds(0.5f);
        _instance.tooltip.SetText(content,header);
        _instance.tooltip.gameObject.SetActive(true);
    }

    
}
