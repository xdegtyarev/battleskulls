using UnityEngine;
using System.Collections;

public class GlassesBoost : ActiveBoost {
	public float duration;
	public float timeToEndHighlight;
	public bool isHighlighting;
	public static GlassesBoost Instance;
	public void Awake(){
		base.Awake();
		Instance = this;
	}

	public void Use(){
		base.Use();
		Debug.Log ("Highlighting");
		Board.Instance.HighlightAllMatchingTurns();
		isHighlighting = true;
		timeToEndHighlight = Time.timeSinceLevelLoad + duration;
	}

	//Call it on any board change action 

	public void UpdateView()
	{
		if (isHighlighting) {
			Board.Instance.DehighlightAllMatcingTurns ();
			Board.Instance.HighlightAllMatchingTurns ();
		}
	}

	public void Update(){
		if(isActive){
		if(isHighlighting){
			if(Time.timeSinceLevelLoad > timeToEndHighlight){
				Board.Instance.DehighlightAllMatcingTurns();
				isHighlighting = false;
			}
		}
		}
	}
}
