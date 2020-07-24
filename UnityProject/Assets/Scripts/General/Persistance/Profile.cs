using UnityEngine;

public class Profile : MonoBehaviour {
	public float[] scores;
	public string[] debugNames;
	public GUIText text;
	public int cascadeCounter;
	public float basicCascadeBonus = 0.2f;
	public float basicScorePerGem = 1f;
	public void IncreaseCascade()
	{
		cascadeCounter++;
	}

	public void ResetCascade()
	{
		cascadeCounter = -1;
	}

	public void Start()
	{
		scores = new float[GemFactory.ThiefItemId-1];
	}
	public void AddScorePoints(int id, int quantity)
	{
		scores [id] += quantity * (basicScorePerGem * (1 + cascadeCounter * basicCascadeBonus));
		UpdateScoreView ();
	}
	
	void UpdateScoreView()
	{
		string s = "";
		for(int i = 0; i<scores.Length; i++){
			s = s + "\n "+ debugNames[i] + " : " + scores[i].ToString("0.0");
		}
		//		text.text = "Scores" + s + "\n Last Cascade Count: " + cascadeCounter;
	}
}
