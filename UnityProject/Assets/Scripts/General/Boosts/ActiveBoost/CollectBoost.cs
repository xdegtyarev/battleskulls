using UnityEngine;
using System.Collections;

public class CollectBoost : ActiveBoost {
	public void Awake(){
		base.Awake();
		Instance = this;
	}
	
	public void Use(){
		base.Use();
		int id;
		if(OffenseBoost.Instance.isActive)
		{
			id = Random.Range(1,4);
		}
		else if(DefenseBoost.Instance.isActive)
		{
			id = Random.Range(4,7);
		}
		else
		{
			id = Random.Range(1,GemFactory.instance.MaxGemIndex);
		}

		Debug.Log ("Collecting:" + id);
		for(int i = 0; i < Board.Size; i++){
			for(int j = 0; j < Board.Size; j++){
				if(Board.Instance.boardData[i,j] == id)
					Board.Instance.board[i,j].Pop();
			}
		}
		AbilityManager.LaunchAbilities();
		Board.Instance.FillBoard();

	}
}
