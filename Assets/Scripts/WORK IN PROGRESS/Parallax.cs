using UnityEngine;

namespace Utilities
{
    public class Parallax : MonoBehaviour
    {
        public GameObject[] levels;
        public float choke; 
        
        private Camera _mainCamera;
        private Vector2 _screenBounds;

        private Vector3 _lastScreenPosition;

        void Start()
        {
            _mainCamera = gameObject.GetComponent<Camera>();
            _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
            foreach (GameObject obj in levels)
            {
                LoadChildObjects(obj);
            }
            _lastScreenPosition = transform.position;
        }
        void LoadChildObjects(GameObject obj)
        {
            float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
            int childsNeeded = (int)Mathf.Ceil(_screenBounds.x * 2 / objectWidth);
            GameObject clone = Instantiate(obj);
            for (int i = 0; i <= childsNeeded; i++)
            {
                GameObject c = Instantiate(clone, obj.transform, true);
                var position = obj.transform.position;
                c.transform.position = new Vector3(objectWidth * i, position.y, position.z);
                c.name = obj.name + i;
            }
            Destroy(clone);
            Destroy(obj.GetComponent<SpriteRenderer>());
        }
        void RepositionChildObjects(GameObject obj)
        {
            Transform[] children = obj.GetComponentsInChildren<Transform>();
            if (children.Length > 1)
            {
                GameObject firstChild = children[1].gameObject;
                GameObject lastChild = children[children.Length - 1].gameObject;
                float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
                if (transform.position.x + _screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
                {
                    firstChild.transform.SetAsLastSibling();
                    var position = lastChild.transform.position;
                    firstChild.transform.position = new Vector3(position.x + halfObjectWidth * 2, position.y, position.z);
                }
                else if (transform.position.x - _screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
                {
                    lastChild.transform.SetAsFirstSibling();
                    var position = firstChild.transform.position;
                    lastChild.transform.position = new Vector3(position.x - halfObjectWidth * 2, position.y, position.z);
                }
            }
        }   
        void LateUpdate()
        {
            foreach (GameObject obj in levels)
            {
                RepositionChildObjects(obj);
                var position = transform.position;
                var parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(position.z / obj.transform.position.z));
                var difference = position.x - _lastScreenPosition.x;
                obj.transform.Translate(Vector3.right * difference * parallaxSpeed);
            }
            _lastScreenPosition = transform.position;
        }
    }
}