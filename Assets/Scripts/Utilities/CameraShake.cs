using Cinemachine;
using UnityEngine;

namespace Utilities
{
	public class CameraShake : MonoBehaviour
	{
		#region Variable Declarations

		public static CameraShake Instance { get; private set; }
		private CinemachineVirtualCamera virtualCam;
		private float shakeTimer;
		private CinemachineBasicMultiChannelPerlin shake;

		#endregion
			
		private void Awake()
		{
			Instance = this;
			virtualCam = GetComponent<CinemachineVirtualCamera>();
			shake = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

		public void ShakeCamera(float intensity,float time)
		{
			shake.m_AmplitudeGain = intensity;
			shakeTimer = time;
		}

		private void Update()
		{
			if (shakeTimer > 0)
			{
				shakeTimer -= Time.deltaTime;
				if (shakeTimer <= 0)
				{
					shake.m_AmplitudeGain = 0f;
				}
			}
		}
	}
}
