using System;
using UnityEngine;
public class VoidAbility: Ability
{
	public override void Use (Vector2i trg)
	{

		int id = Board.Instance.boardData[trg.x,trg.y];
		Debug.Log ("Destroing all " + id);
		base.Use (trg);
		for(int i = 0; i < Board.Size; i++){
			for(int j = 0; j < Board.Size; j++){
				if(Board.Instance.boardData[i,j] == id)
				{
					Board.Instance.board[i,j].Pop();
				}
			}
		}
	}
}


