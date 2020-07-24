using UnityEngine;
using System.Collections;

public class SpeedBoost : ActiveBoost {
	public float speedValue;
	public float duration;
	public bool isSpeeded;
	public float timeToSlowDown;
	public static SpeedBoost Instance;
	public void Awake(){
		base.Awake();
		Instance = this;
	}
	public void Update()
	{
		if(isActive){
			if (isSpeeded){
				if(Time.timeSinceLevelLoad >= timeToSlowDown){
					SlowDown();
				}
			}
		}
	}

	public void Use()
	{
		base.Use();
		SpeedUp();
	}

	void SpeedUp(){
		isSpeeded = true;
		timeToSlowDown = Time.timeSinceLevelLoad + duration;

	}
	void SlowDown(){
		isSpeeded = false;
	}
}
