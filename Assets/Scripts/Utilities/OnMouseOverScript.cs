using System.Collections;
using Managers;
using UnityEngine;

namespace Utilities
{
    public class OnMouseOverScript : MonoBehaviour
    {
        private bool coroutineAllowed;
        private Vector2 localScale;
        private void Start()
        {
            coroutineAllowed = true;
            localScale = transform.localScale;
        }

        private void OnMouseOver()
        {
            if (coroutineAllowed)
            {
                StartCoroutine("StartPulsing");
            }
        }

        private IEnumerator StartPulsing()
        {
            coroutineAllowed = false;

            for (float i = 0f; i <= 1f; i+=0.2f)
            {
                transform.localScale = new Vector3(
                    (Mathf.Lerp(localScale.x, localScale.x + 0.2f, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(localScale.y, localScale.y + 0.2f, Mathf.SmoothStep(0f, 1f, i))));
                yield return  new WaitForSeconds(0.05f);
            }
       
            for (float i = 0; i <= 1f; i+=0.2f)
            {
                transform.localScale = new Vector3(
                    (Mathf.Lerp(localScale.x + 0.2f, localScale.x , Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(localScale.y + 0.2f, localScale.y, Mathf.SmoothStep(0f, 1f, i))));
                yield return  new WaitForSeconds(0.05f);
            }

            coroutineAllowed = true;

        }

        private void OnMouseEnter()
        {
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Attack);
        }

        private void OnMouseExit()
        {
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);

        }
    }
}
