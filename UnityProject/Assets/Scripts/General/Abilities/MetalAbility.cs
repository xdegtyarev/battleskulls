using System;
using UnityEngine;
public class MetalAbility: Ability
{
	public override void Use (Vector2i trg)
	{
		Debug.Log ("Slash at:" + trg.x + " " + trg.y);
		base.Use (trg);
		int i = 0;
		while(Board.Instance.FitsBoardBounds(new Vector2i(trg.x + i, trg.y - i))){
			if(Board.Instance.boardData[trg.x + i, trg.y - i] > 0){
				Board.Instance.board[trg.x + i, trg.y - i].Pop();
			}
			i++;
		}
		i = 0;
		while(Board.Instance.FitsBoardBounds(new Vector2i(trg.x + i, trg.y - i))){
			if(Board.Instance.boardData[trg.x + i, trg.y - i] > 0){
				Board.Instance.board[trg.x + i, trg.y - i].Pop();
			}
			i--;
		}
	}
}


