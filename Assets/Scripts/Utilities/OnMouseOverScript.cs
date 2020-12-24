using System.Collections;
using Managers;
using UnityEngine;

namespace Utilities
{
    public class OnMouseOverScript : MonoBehaviour
    {
        private bool _coroutineAllowed;
        private Vector2 _localScale;
#pragma warning disable 0649
        [SerializeField] private GameObject targetAnim;
#pragma warning restore 0649
        private void Start ()
        {
            _coroutineAllowed = true;
            _localScale = transform.localScale;
        }
        private void OnMouseDown ()
        {
            GameObject targetGO = Instantiate (targetAnim, transform.position + new Vector3 (0f, 3f, 0f), Quaternion.identity);
            Destroy (targetGO, 2f);
        }

        private void OnMouseOver ()
        {
            if (_coroutineAllowed)
            {
                StartCoroutine ("StartPulsing");
            }
        }

        private IEnumerator StartPulsing ()
        {
            _coroutineAllowed = false;

            for (float i = 0f; i <= 1f; i += 0.2f)
            {
                transform.localScale = new Vector3 (
                    (Mathf.Lerp (_localScale.x, _localScale.x + 0.2f, Mathf.SmoothStep (0f, 1f, i))),
                    (Mathf.Lerp (_localScale.y, _localScale.y + 0.2f, Mathf.SmoothStep (0f, 1f, i))));
                yield return new WaitForSeconds (0.05f);
            }

            for (float i = 0; i <= 1f; i += 0.2f)
            {
                transform.localScale = new Vector3 (
                    (Mathf.Lerp (_localScale.x + 0.2f, _localScale.x, Mathf.SmoothStep (0f, 1f, i))),
                    (Mathf.Lerp (_localScale.y + 0.2f, _localScale.y, Mathf.SmoothStep (0f, 1f, i))));
                yield return new WaitForSeconds (0.05f);
            }

            _coroutineAllowed = true;

        }

        private void OnMouseEnter ()
        {
            CursorManager.Instance.SetActiveCursorType (CursorManager.CursorType.ATTACK);
        }

        private void OnMouseExit ()
        {
            CursorManager.Instance.SetActiveCursorType (CursorManager.CursorType.DEFAULT);

        }
    }
}