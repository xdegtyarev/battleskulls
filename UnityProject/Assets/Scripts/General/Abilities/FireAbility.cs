using System;
using UnityEngine;
public class FireAbility: Ability
{
	public override void Use (Vector2i trg)
	{
		Debug.Log ("Vertical at:" + trg.x + " " + trg.y);
		base.Use (trg);
		for(int i = 0; i < Board.Size; i++){
			if(Board.Instance.boardData[trg.x,i] > 0){
				Board.Instance.board[trg.x,i].Pop();
			}
		}
	}
}