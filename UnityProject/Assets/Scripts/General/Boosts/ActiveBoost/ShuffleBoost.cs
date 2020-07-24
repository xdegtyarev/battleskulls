using UnityEngine;
using System.Collections.Generic;

public class ShuffleBoost : ActiveBoost {
	public void Awake(){
		base.Awake();
		Instance = this;
	}

	public struct GemInfo
	{
		public int id;
		public GemType type;
	}
	
	public void Use()
	{
		base.Use();
		List<GemInfo> list = new List<GemInfo>();
		//Collecting Info
		for(int i = 0; i<Board.Size; i++){
			for(int j = 0; j<Board.Size; j++){
				if(Board.Instance.boardData[i,j] > 0){
					list.Add(new GemInfo{id = Board.Instance.board[i,j].id, type = Board.Instance.board[i,j].type});
				}
			}
		}
		//Placing Randomly
		for(int i = 0; i<Board.Size; i++){
			for(int j = 0; j<Board.Size; j++){
				if(Board.Instance.boardData[i,j]>0){
					Gem trg = Board.Instance.board[i,j];
					Destroy(trg.view);
					int index = 0;
					while(list[index].id <= 0){
						index = Random.Range(0,list.Count);
					}

					trg.Init(list[index].type,list[index].id);
					Vector2i v2i = new Vector2i(trg.transform.position);
					Board.Instance.boardData [v2i.x, v2i.y] = list[index].id;
					list[index] = new GemInfo{id = 0, type = GemType.general};
				}
			}
		}
		//Finding Matches
		List<Gem> allMatches = new List<Gem>();
		List<Gem> matches;
		for(int i = 0; i<Board.Size; i++){
			for(int j = 0; j<Board.Size; j++){
				Board.Instance.CheckMatch (new Vector2i (i, j));
			}
		}
		AbilityManager.LaunchAbilities();
		Board.Instance.FillBoard();
	}
}
