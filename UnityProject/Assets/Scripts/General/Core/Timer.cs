using UnityEngine;


public class Timer : MonoBehaviour {
	float currentTime;
	public float maxGameplayTime = 60f;
	public GUIText DebugText;
	void Start () {
	
	}

	void Update () {
		currentTime = Time.timeSinceLevelLoad;
		DebugText.text = "Time Left: " + (int)(maxGameplayTime - currentTime) + " sec"; 
		if (maxGameplayTime - currentTime < 0) {
			Application.LoadLevel(0);
		}
	}
}
