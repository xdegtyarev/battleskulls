using UnityEngine;

public class Game : MonoBehaviour {
	public static Game instance;
	public Profile currentProfile;
	void Awake()
	{
		instance = this;
	}
}
