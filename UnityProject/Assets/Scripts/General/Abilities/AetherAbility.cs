using System;
using UnityEngine;
public class AetherAbility: Ability
{
	public override void Use (Vector2i trg)
	{
		Debug.Log ("Diamond at:" + trg.x + " " + trg.y);
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
		if(Board.Instance.FitsBoardBounds(new Vector2i(trg.x + 2, trg.y))){
			if(Board.Instance.boardData[trg.x + 2, trg.y] > 0){
				Board.Instance.board[trg.x + 2, trg.y].Pop();
			}
		}
		if(Board.Instance.FitsBoardBounds(new Vector2i(trg.x - 2, trg.y))){
			if(Board.Instance.boardData[trg.x - 2, trg.y] > 0){
				Board.Instance.board[trg.x - 2, trg.y].Pop();
			}
		}
		if(Board.Instance.FitsBoardBounds(new Vector2i(trg.x, trg.y+2))){
			if(Board.Instance.boardData[trg.x, trg.y+2] > 0){
				Board.Instance.board[trg.x, trg.y+2].Pop();
			}
		}
		if(Board.Instance.FitsBoardBounds(new Vector2i(trg.x, trg.y-2))){
			if(Board.Instance.boardData[trg.x, trg.y-2] > 0){
				Board.Instance.board[trg.x, trg.y-2].Pop();
			}
		}
	}
}


