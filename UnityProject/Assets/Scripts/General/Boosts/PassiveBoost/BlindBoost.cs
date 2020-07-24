using UnityEngine;

public class BlindBoost : Boost {
	public int Quantity = 1;
	public float delay = 5f;
	float nextTimeToSuggest = 0f; 
	TouchProcessor touchProcessor;
	public static BlindBoost Instance;
	void Awake(){
		base.Awake();
		Instance = this;
		touchProcessor = TouchProcessor.Instance;
		nextTimeToSuggest = delay*2;
	}
	void Update()
	{
		if(isActive){
			if(!GlassesBoost.Instance.isHighlighting){
				if (touchProcessor.isSelected) {
					if(Board.Instance.isHighlighted){
						Board.Instance.DehighlightAllMatcingTurns();
					}
					UpdateNextTimeToSuggest();
				} else {
					if(Time.timeSinceLevelLoad >= nextTimeToSuggest){
						UpdateNextTimeToSuggest();
						Board.Instance.HighlightMatchingTurns(Quantity);
					}
				}
			}
		}
	}

	public void UpdateNextTimeToSuggest(){
		nextTimeToSuggest = Time.timeSinceLevelLoad + delay;
	}
}
