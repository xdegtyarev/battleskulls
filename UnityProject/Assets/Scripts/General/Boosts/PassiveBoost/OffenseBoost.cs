using UnityEngine;
using System.Collections;

public class OffenseBoost : Boost {
	public float chance = 0.25f;
	public static OffenseBoost	Instance;
	void Awake(){
		base.Awake();
		Instance = this;
	}
}
