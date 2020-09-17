using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "Cursor", menuName = "Cursor", order = 0)]
	public class CursorAnimation : ScriptableObject
	{
		public CursorManager.CursorType cursorType;
		public Texture2D[] textureArray;
		public float frameRate;
		public Vector2 hotspot;
	}
}