using UnityEngine;

public class TouchInfo
{
	public enum TouchMoveDirection
	{
		UP,
		RIGHT,
		DOWN,
		LEFT,
		OTHER
	}
	public static float StationaryDelta = 4f;
	public static float MaxAlphaAngle = Mathf.Deg2Rad * 45f;
	public static float MaxSnapDelta = Mathf.Sin (MaxAlphaAngle);
	public int id;
	public TouchPhase phase;
	public Vector2 delta;
	public Vector2 position;
	public Vector2 previousPosition;

	public void UpdatePosition (Vector2 newPosition)
	{
		previousPosition = position;
		position = newPosition;
		delta = previousPosition - position;
	}

	public bool IsStationary{ get { return delta.magnitude < StationaryDelta; } }

	public TouchMoveDirection GetTouchMoveDirection ()
	{
		Vector2 n = delta.normalized;

		if (Mathf.Abs (n.y) + MaxSnapDelta > 1f && Mathf.Abs (n.x) < MaxSnapDelta) {
			if (n.y > 0f) {
				return TouchMoveDirection.DOWN;
			} 
			if (n.y < 0f) {
				return TouchMoveDirection.UP;
			}
		} 

		if (Mathf.Abs (n.x) + MaxSnapDelta > 1f && Mathf.Abs (n.y) < MaxSnapDelta) {
			if (n.x > 0f) {
				return TouchMoveDirection.LEFT;
			}
			if (n.x < 0f) {
				return TouchMoveDirection.RIGHT;
			}
		}

		return TouchMoveDirection.OTHER;
	}
}
