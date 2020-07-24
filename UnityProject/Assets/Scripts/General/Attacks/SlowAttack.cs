using UnityEngine;
using System.Collections;
public class SlowAttack: Attack{
	public float duration;
	public float slowValue;
	public bool isSlowed;
	public static SlowAttack Instance;

	public void Awake(){
		base.Awake ();
		Instance = this;
	}

	public IEnumerator Start(){
		if (isActive) {
			isSlowed = true;
			yield return new WaitForSeconds (duration);
			isSlowed = false;
		}
	}
}

