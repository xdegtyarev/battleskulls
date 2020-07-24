using UnityEngine;
using System.Collections;

public class DefenseBoost : Boost {
	public float chance = 0.25f;
	public static DefenseBoost Instance;
	void Awake(){
		base.Awake();
		Instance = this;
	}
}
