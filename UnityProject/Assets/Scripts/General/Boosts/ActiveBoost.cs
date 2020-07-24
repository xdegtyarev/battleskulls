using UnityEngine;
using System.Collections;

public abstract class ActiveBoost : Boost {
	public int usesCount;
	public GameObject Button;
	public void Awake(){
		base.Awake();
		if(isActive){
			Button.SetActive(true);
		}
	}

	public void Use(){
		usesCount--;
		if(usesCount == 0)
			Button.SetActive(false);

		if(BlindBoost.Instance.isActive)
		{
			BlindBoost.Instance.UpdateNextTimeToSuggest();
			if(Board.Instance.isHighlighted){
				Board.Instance.DehighlightAllMatcingTurns();
			}
		}

	}
}
