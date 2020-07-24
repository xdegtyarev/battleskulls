using System;
using UnityEngine;
public class EarthAbility: Ability
{
	public override void Use (Vector2i trg)
	{
		Debug.Log ("Boom at:" + trg.x + " " + trg.y);
		base.Use (trg);
		for(int i = -1; i < 2; i++){
			for(int j = -1; j < 2; j++){
				if(Board.Instance.FitsBoardBounds(new Vector2i(trg.x + i, trg.y + j))){
					if(Board.Instance.boardData[trg.x + i, trg.y + j] > 0){
						Board.Instance.board[trg.x + i, trg.y + j].Pop();
					}
				}
			}
		}
	}
}


