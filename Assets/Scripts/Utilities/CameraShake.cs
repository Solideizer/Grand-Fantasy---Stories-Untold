using Cinemachine;
using UnityEngine;

namespace Utilities
{
	public class CameraShake : MonoBehaviour
	{
		#region Variable Declarations

		public static CameraShake Instance { get; private set; }
		private CinemachineVirtualCamera _virtualCam;
		private float _shakeTimer;
		private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

		#endregion
			
		private void Awake()
		{
			Instance = this;
			_virtualCam = GetComponent<CinemachineVirtualCamera>();
			_multiChannelPerlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

		public void ShakeCamera(float intensity,float time)
		{
			_multiChannelPerlin.m_AmplitudeGain = intensity;
			_shakeTimer = time;
		}

		private void Update()
		{
			if (!(_shakeTimer > 0)) return;
			_shakeTimer -= Time.deltaTime;
			if (_shakeTimer <= 0)
			{
				_multiChannelPerlin.m_AmplitudeGain = 0f;
			}
		}
	}
}
