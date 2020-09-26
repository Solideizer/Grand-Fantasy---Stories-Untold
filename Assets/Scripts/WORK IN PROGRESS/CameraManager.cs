using System.Collections;
using Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        private Vector2 defaultPosition;
        public CinemachineVirtualCamera cam;
        private void Awake()
        {
            defaultPosition = new Vector3(0f,0f,-10f);

        }
        public IEnumerator MoveTowardsTarget(GameObject target)
        {
            Vector2 targetPos = target.transform.position;
            var transformPosition = defaultPosition;
            
            if (target.CompareTag("EnemyUnit"))
            {
                Mathf.Lerp(transformPosition.x, targetPos.x + 10f, 20f);
            }
            else
            {
                Mathf.Lerp(transformPosition.x, targetPos.x - 10f, 20f);
            }
            Mathf.Lerp(transformPosition.y,targetPos.y,0.05f);;
            
            
            cam.m_Lens.OrthographicSize = Mathf.Lerp(7,5,1f);
            yield return new WaitForSeconds(1f);
            
            if (target.CompareTag("EnemyUnit"))
            {
                Mathf.Lerp(targetPos.x + 10f,transform.position.x,5f);
            }
            else
            {
                Mathf.Lerp(targetPos.x - 10f,transform.position.x,5f);
            }
            Mathf.Lerp(targetPos.y,transform.position.y,0.05f);
            cam.m_Lens.OrthographicSize = Mathf.Lerp(5,7,1f);
        }

       
    }
}
