using UnityEngine;
public class WaterAbility: Ability
{
	public override void Use (Vector2i trg)
	{
		base.Use (trg);
		Debug.Log ("Horizontal at:" + trg.x + " " + trg.y);
		for(int i = 0; i < Board.Size; i++){
			if(Board.Instance.boardData[i,trg.y] > 0){
				Board.Instance.board[i,trg.y].Pop();
			}
		}
	}
}


