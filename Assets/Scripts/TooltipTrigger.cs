using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
	[SerializeField] private string header;
	[SerializeField][TextArea] private string content;
	
		public void OnPointerEnter(PointerEventData eventData)
		{
			StartCoroutine(TooltipManager.ShowTooltip(content, header));
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			TooltipManager.Hide();
		}
	}
