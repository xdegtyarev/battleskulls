using UnityEngine;

public struct Vector2i{
	public int x,y;
	public Vector2i(Vector3 v3)
	{
		x = (int)v3.x;
		y = (int)v3.y;
	}
	public Vector2i(int _x,int _y)
	{
		x = _x;
		y = _y;
	}
}