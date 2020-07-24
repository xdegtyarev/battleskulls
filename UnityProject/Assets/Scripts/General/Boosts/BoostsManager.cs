using UnityEngine;
using System.Collections;

public class BoostsManager : MonoBehaviour {
	public Boost[] boosts;
	public static BoostsManager Instance;
	void Awake(){
		Instance = this;
	}
}
