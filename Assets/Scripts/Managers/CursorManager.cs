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

		private int currentFrame;
		private float frameTimer;
		private int frameCount;
		private CursorAnimation cursorAnimation;

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
			frameTimer -= Time.deltaTime;
			if (frameTimer <= 0f)
			{
				frameTimer += cursorAnimation.frameRate;
				currentFrame = (currentFrame + 1) % frameCount;
				Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.hotspot,
					CursorMode.ForceSoftware);
			}
		}

		#endregion

		private void SetActiveCursorAnimation(CursorAnimation cursorAnim)
		{
			this.cursorAnimation = cursorAnim;
			currentFrame = 0;
			frameTimer = 0;
			frameCount = cursorAnimation.textureArray.Length;
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