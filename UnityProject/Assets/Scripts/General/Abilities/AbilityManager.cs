using System;
using System.Collections.Generic;
using UnityEngine;
public class AbilityManager: MonoBehaviour
{
	public class AbilityLaunchInfo
	{
		public Vector2i trg;
		public int id;
	}

	public static AbilityManager Instance;
	public Ability[] abilities;
	public List<AbilityLaunchInfo> abilitiesToLaunch = new List<AbilityLaunchInfo>();
	void Awake(){
		Instance = this;
	}

	public static Ability GetAbility(int id)
	{
		return Instance.abilities[id-1];	
	}

	public static void AddAbilityToLaunch(int id, Vector2i trg)
	{
		var o = new AbilityLaunchInfo();
		o.id = id;
		o.trg = trg;
		if(isLaunchingAbilities){
			GetAbility(id).Use (trg);
		}
		else{
			Instance.abilitiesToLaunch.Add(o);
		}
	}
	static bool isLaunchingAbilities;
	public static void LaunchAbilities()
	{
		isLaunchingAbilities = true;
		for (int i = 0; i < Instance.abilitiesToLaunch.Count; i++) {
			var o = Instance.abilitiesToLaunch [i];
			GetAbility (o.id).Use (o.trg);
		}
		Instance.abilitiesToLaunch.Clear();
		isLaunchingAbilities = false;
	}
}


