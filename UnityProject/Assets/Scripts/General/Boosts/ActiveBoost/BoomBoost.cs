using UnityEngine;
using System.Collections;

public class BoomBoost : ActiveBoost {
	public int squareSize = 3;
	public void Awake(){
		base.Awake();
		Instance = this;
	}
	
	public void Use(){
		base.Use();
		int x = Random.Range(1,Board.Size - 1);
		int y = Random.Range(1,Board.Size - 1);
		Debug.Log ("Bomb in:" + x + ":" + y);

		for(int i = x - 1; i <= x + 1; i++){
			for(int j = y - 1; j <= y + 1; j++){
				if(Board.Instance.board[i,j] != null){
					Board.Instance.board[i,j].Pop();
				}
			}
		}
		AbilityManager.LaunchAbilities ();
		Board.Instance.FillBoard();
	}
}
