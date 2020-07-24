using UnityEngine;
using System.Collections;

public abstract class Boost : MonoBehaviour {
	public bool isActive;
	public static Boost Instance;
	// Use this for initialization
	internal void Awake () {
		isActive = PlayerPrefs.GetInt(name) == 1;
	}
}
