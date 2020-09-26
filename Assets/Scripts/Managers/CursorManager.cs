using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class CursorManager : MonoBehaviour
	{
		#region Variable Declarations

#pragma warning disable 0649
		[SerializeField] private List<CursorAnimation> cursorAnimationList;
#pragma warning restore 0649
		public static CursorManager Instance { get; private set; }

		private int _currentFrame;
		private float _frameTimer;
		private int _frameCount;
		private CursorAnimation _cursorAnimation;

		public enum CursorType
		{
			Default,
			Attack
		}

		#endregion

		#region Awake-Start-Update

		private void Awake()
		{
			Instance = this;
		}

		void Start()
		{
			SetActiveCursorType(CursorType.Default);
		}

		private void Update()
		{
			_frameTimer -= Time.deltaTime;
			if (_frameTimer <= 0f)
			{
				_frameTimer += _cursorAnimation.frameRate;
				_currentFrame = (_currentFrame + 1) % _frameCount;
				Cursor.SetCursor(_cursorAnimation.textureArray[_currentFrame], _cursorAnimation.hotspot,
					CursorMode.ForceSoftware);
			}
		}

		#endregion

		private void SetActiveCursorAnimation(CursorAnimation cursorAnim)
		{
			this._cursorAnimation = cursorAnim;
			_currentFrame = 0;
			_frameTimer = 0;
			_frameCount = _cursorAnimation.textureArray.Length;
		}

		public void SetActiveCursorType(CursorType cursorType)
		{
			SetActiveCursorAnimation(GetCursorAnimation(cursorType));
		}

		private CursorAnimation GetCursorAnimation(CursorType cursorType)
		{
			foreach (CursorAnimation cursorAnim in cursorAnimationList)
			{
				if (cursorAnim.cursorType == cursorType)
				{
					return cursorAnim;
				}
			}

			return null;
		}
	}
}