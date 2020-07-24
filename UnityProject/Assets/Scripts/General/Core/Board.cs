using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Board : MonoBehaviour
{
	public bool autoTurns;
	public static int Size = 8;
	public static Board Instance;
	public List<Gem>[] columns;
	public Gem[,] board;
	public int[,] boardData;
	bool isSwapping;
	int swapCounter;
	bool swapBack;
	Vector2i _src;
	Vector2i _trg;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		InitBoard ();
	}

	void InitBoard ()
	{
		board = new Gem[Size, Size];
		boardData = new int[Size, Size];
		columns = new List<Gem>[Size];
		for (int i = 0; i < Size; i++) {
			columns [i] = new List<Gem> ();
		}
		InitialBoardFill ();
	}

	void InitialBoardFill ()
	{
		do {
			for (int i = 0; i < Size; i++) {
				for (int j = 0; j < Size; j++) {
					boardData [i, j] = Random.Range (1, GemFactory.instance.MaxGemIndex - 1);
				}
			}
		} while(HasMatches ());
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				board [i, j] = GemFactory.instance.SpawnGem (i, j, boardData [i, j]);
			}
		}
	}

	bool HasMatches ()
	{
		var l = new List<Gem> ();
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				if (TryFindEqualMatchingNeighbours (i, j, out l)) {
					return true;
				}
			}
		}
		return false;
	}

	public bool FitsBoardBounds (Vector2i v)
	{
		return v.x >= 0 && v.y >= 0 && v.x < Size && v.y < Size;
	}

	public void Swap (Vector2i src, Vector2i trg)
	{
		if (isSwapping) {
			return;
		}

		if (columns [src.x].Count > 0 || columns [trg.x].Count > 0) {
			return;
		}

		if (FitsBoardBounds (src) && FitsBoardBounds (trg)) {

			Gem srcGem = board [src.x, src.y];
			Gem trgGem = board [trg.x, trg.y];

			if (srcGem == null || srcGem.isMoving || srcGem.isFalling) {
				return;
			}
			if (trgGem == null || trgGem.isMoving || trgGem.isFalling) {
				return;
			}


			Game.instance.currentProfile.ResetCascade ();

			isSwapping = true;

			Deregister (src, 0);
			srcGem.Move (trg);
			Deregister (trg, 0);
			trgGem.Move (src);
		}
	}

	public bool CheckMatch (Vector2i v2i)
	{
		List<Gem> v2iGems;
		if (TryFindEqualMatchingNeighbours (v2i.x, v2i.y, out v2iGems)) { 
			if (v2iGems == null) {
				return false;
			} else {
				Game.instance.currentProfile.IncreaseCascade ();
				foreach (var o in v2iGems) {
					o.Pop ();
				}
				return true;
			}
		}
		return false;
	}

	public void OnSwapComplete (Vector2i v2i)
	{
		if (swapBack) {
			if (swapCounter == 0) {
				swapCounter++;
			} else {
				swapCounter = 0;
				swapBack = false;
				isSwapping = false;
			}
		} else {
			if (swapCounter == 0) {
				_src = v2i;
				swapCounter++;
			} else {
				_trg = v2i;
				bool srcResult;
				bool trgResult;

				srcResult = CheckMatch (_src);
				trgResult = CheckMatch (_trg);
					
				if (srcResult || trgResult) {
					isSwapping = false;
					AbilityManager.LaunchAbilities ();
					FillBoard ();
				} else {
					board [_src.x, _src.y].Move (_trg);
					Deregister (_src, 0);
					board [_trg.x, _trg.y].Move (_src);
					Deregister (_trg, 0);
					swapBack = true;
				}
				swapCounter = 0;
			}
		}
	}

	int fallingGemCounter;
	List<Vector2i> fallingGemsList = new List<Vector2i> ();

	public void LogFallingGem (Vector2i gem)
	{
		fallingGemCounter--;
		columns [gem.x].Remove (board [gem.x, gem.y]);
		fallingGemsList.Add (gem);

		if (fallingGemCounter < 0) {
			Debug.LogError ("SMTH Went wrong: falling gem counter:" + fallingGemCounter);
		}
	
	}

	public int pointsForResettingBoard = 10;

	void EndTurn ()
	{
		var turns = FindAllMatchingTurns ();
		if (turns == 0 && fallingGemCounter == 0) {
			Debug.Log ("NO MORE TURNS: already made: " + turn + " turns");
			StartCoroutine (ResetBoardCoroutine ());
		}
	}

	bool isResetting;

	IEnumerator ResetBoardCoroutine ()
	{
		isResetting = true;
		yield return new WaitForSeconds (2f);
		Game.instance.currentProfile.ResetCascade ();
		Game.instance.currentProfile.IncreaseCascade ();
		ResetBoard ();
		Game.instance.currentProfile.AddScorePoints (GemFactory.ThiefItemId - 2, pointsForResettingBoard);
		isResetting = false;
		yield return null;
	}

	void Update ()
	{
		if (fallingGemsList.Count > 0) {
			bool hasMatches = false;
			foreach (Vector2i v in fallingGemsList) {
				if (board [v.x, v.y] != null) {
					if(CheckMatch (v)) {
						hasMatches = true;
					}
					if (isSwapping) {
					} else {
						//AbilityManager.LaunchAbilities ();
						FillBoard ();
					}
				}
			}
			fallingGemsList.Clear ();
			if (!hasMatches) {
				EndTurn ();
			}
		} 

		if (autoTurns) {
			if (!isResetting)
				MakeTurn ();
		}
	}

	public void FillBoard ()
	{
		List<Gem> calculatedChunk;
		int emptyCellCounter;
		int targetHeight;
		for (int i = 0; i < Size; i++) {
			emptyCellCounter = 0;
			calculatedChunk = new List<Gem> ();
			targetHeight = -1;
			for (int j = 0; j < Size; j++) {
				if (boardData [i, j] == 0) {
					if (targetHeight == -1) {
						targetHeight = j; //Only once to count fall values during one turn;
					}
					boardData [i, j] = -1; //MARKING THAT THIS ONE WILL BE FILLED right now and once;

					emptyCellCounter++;
				} else if (boardData [i, j] != -1) {
					calculatedChunk.Add (board [i, j]);
					fallingGemCounter++;
				} 
			}
			if (emptyCellCounter > 0) {
				fallingGemCounter += emptyCellCounter;
				calculatedChunk.AddRange (columns [i]); //Adding to the end of calculated chunks too keep order.
				calculatedChunk.AddRange (GemFactory.instance.SpawnGems (i, emptyCellCounter)); //Addding generated items
				columns [i] = calculatedChunk;
				if(columns.Length>Size){
					Debug.Break();
					Debug.Log("Colums size is greater than board size");
				}
				int k = 0;
				foreach (Gem o in columns[i]) {
					StartCoroutine (o.SetFallingValue (k, 0.5f));
					k++;
				}
			}
		}
	}

	public void ResetBoard ()
	{
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				if (board [i, j] != null) {
					board [i, j].DestroySelf ();
				}
			}
		}
		InitialBoardFill ();
	}

	public void Register (Gem gem)
	{
		var v2i = new Vector2i (gem.transform.position);
		if (!FitsBoardBounds (v2i)) {
			Debug.Break ();
			Debug.Log ("v2i: " + v2i.x + " " + v2i.y);
			Debug.Log ("chunks count :" + columns [v2i.x].Count);
		}
		if (board [v2i.x, v2i.y] != null) {
			Debug.LogError ("Cell is not empty at:" + v2i.x + " " + v2i.y);
		} else {
			board [v2i.x, v2i.y] = gem;		
			boardData [v2i.x, v2i.y] = gem.id;
		}
		
	}

	public void Deregister (Vector2i v2i, int value)
	{
		Debug.Log ("Deregistering : " + v2i.x + " : " + v2i.y + ".");
		board [v2i.x, v2i.y] = null;
		boardData [v2i.x, v2i.y] = value;
	}

	public bool TryFindEqualMatchingNeighbours (int x, int y, out List<Gem> gems)
	{
		List<Gem> VNeighbours = new List<Gem> ();
		List<Gem> HNeighbours = new List<Gem> ();

		int value = boardData [x, y];
		if (value == 0 || value == -1) {
			gems = null;
			return false;
		}
		for (int i = y + 1; i < Size && boardData [x, i] == value; i++) {
			VNeighbours.Add (board [x, i]);
		}
		for (int i = y - 1; i >= 0 && boardData [x, i] == value; i--) {
			VNeighbours.Add (board [x, i]);
		}
		for (int i = x + 1; i < Size && boardData [i, y] == value; i++) {
			HNeighbours.Add (board [i, y]);
		}
		for (int i = x - 1; i >= 0 && boardData [i, y] == value; i--) {
			HNeighbours.Add (board [i, y]);
		}

		if (VNeighbours.Count >= 2 && HNeighbours.Count >= 2) {
			VNeighbours.AddRange (HNeighbours);
			VNeighbours.Add (board [x, y]);
			gems = VNeighbours;
			return true;
		} else {
			if (VNeighbours.Count >= 2) {
				VNeighbours.Add (board [x, y]);
				gems = VNeighbours;
				return true;
			} else if (HNeighbours.Count >= 2) {
				HNeighbours.Add (board [x, y]);
				gems = HNeighbours;
				return true;
			} else {
				gems = null;
				return false;
			}
		}
	}
	//TODO: Wrap to external class with pattern matching
	public class MatchPattern
	{
		public static MatchPattern Empty;
		public Vector2i offset1;
		public Vector2i offset2;
		// acts in swap
		public Vector2i offset3;
		// target of swap
	}
	//TODO: Wrap to external class with pattern matching
	static MatchPattern pattern1 = new MatchPattern {
		offset1 = new Vector2i (2, 0),
		offset2 = new Vector2i (1, 1),
		offset3 = new Vector2i (1, 0)
	};
	static MatchPattern pattern2 = new MatchPattern {
		offset1 = new Vector2i (1, 0),
		offset2 = new Vector2i (2, 1),
		offset3 = new Vector2i (2, 0)
	};
	static MatchPattern pattern3 = new MatchPattern {
		offset1 = new Vector2i (1, 0),
		offset2 = new Vector2i (3, 0),
		offset3 = new Vector2i (2, 0)
	};
	static MatchPattern pattern4 = new MatchPattern {
		offset1 = new Vector2i (1, 0),
		offset2 = new Vector2i (2, -1),
		offset3 = new Vector2i (2, 0)
	};
	static MatchPattern pattern5 = new MatchPattern {
		offset1 = new Vector2i (0, 2),
		offset2 = new Vector2i (-1, 1),
		offset3 = new Vector2i (0, 1)
	};
	static MatchPattern pattern6 = new MatchPattern {
		offset1 = new Vector2i (0, 1),
		offset2 = new Vector2i (-1, 2),
		offset3 = new Vector2i (0, 2)
	};
	static MatchPattern pattern7 = new MatchPattern {
		offset1 = new Vector2i (0, 1),
		offset2 = new Vector2i (0, 3),
		offset3 = new Vector2i (0, 2)
	};
	static MatchPattern pattern8 = new MatchPattern {
		offset1 = new Vector2i (0, 1),
		offset2 = new Vector2i (1, 2),
		offset3 = new Vector2i (0, 2)
	};
	static MatchPattern pattern9 = new MatchPattern {
		offset1 = new Vector2i (-2, 0),
		offset2 = new Vector2i (-1, -1),
		offset3 = new Vector2i (-1, 0)
	};
	static MatchPattern pattern10 = new MatchPattern {
		offset1 = new Vector2i (-1, 0),
		offset2 = new Vector2i (-2, -1),
		offset3 = new Vector2i (-2, 0)
	};
	static MatchPattern pattern11 = new MatchPattern {
		offset1 = new Vector2i (-1, 0),
		offset2 = new Vector2i (-3, 0),
		offset3 = new Vector2i (-2, 0)
	};
	static MatchPattern pattern12 = new MatchPattern {
		offset1 = new Vector2i (-1, 0),
		offset2 = new Vector2i (-2, 1),
		offset3 = new Vector2i (-2, 0)
	};
	static MatchPattern pattern13 = new MatchPattern {
		offset1 = new Vector2i (0, -2),
		offset2 = new Vector2i (1, -1),
		offset3 = new Vector2i (0, -1)
	};
	static MatchPattern pattern14 = new MatchPattern {
		offset1 = new Vector2i (0, -1),
		offset2 = new Vector2i (1, -2),
		offset3 = new Vector2i (0, -2)
	};
	static MatchPattern pattern15 = new MatchPattern {
		offset1 = new Vector2i (0, -1),
		offset2 = new Vector2i (0, -3),
		offset3 = new Vector2i (0, -2)
	};
	static MatchPattern pattern16 = new MatchPattern {
		offset1 = new Vector2i (0, -1),
		offset2 = new Vector2i (-1, -2),
		offset3 = new Vector2i (0, -2)
	};
	//TODO: Wrap to external class with pattern matching
	MatchPattern[] matchPatterns = new MatchPattern[16] {
		pattern1,
		pattern2,
		pattern3,
		pattern4,
		pattern5,
		pattern6,
		pattern7,
		pattern8,
		pattern9,
		pattern10,
		pattern11,
		pattern12,
		pattern13,
		pattern14,
		pattern15,
		pattern16
	};

	bool CheckMatchPattern (int x, int y, MatchPattern pattern)
	{
		if ((x + pattern.offset1.x < Size && x + pattern.offset1.x >= 0) &&
		    (y + pattern.offset1.y < Size && y + pattern.offset1.y >= 0) &&
		    (x + pattern.offset2.x < Size && x + pattern.offset2.x >= 0) &&
		    (y + pattern.offset2.y < Size && y + pattern.offset2.y >= 0) &&
		    (x + pattern.offset3.x < Size && x + pattern.offset3.x >= 0) &&
		    (y + pattern.offset3.y < Size && y + pattern.offset3.y >= 0)) {
			return boardData [x, y] == boardData [x + pattern.offset1.x, y + pattern.offset1.y] &&
			boardData [x, y] == boardData [x + pattern.offset2.x, y + pattern.offset2.y];
		}
		return false;
	}

	public bool TryFindMatchingTurns (int x, int y, out MatchPattern pattern)
	{
		if (boardData [x, y] > 0) {
			foreach (MatchPattern p in matchPatterns) {
				if (CheckMatchPattern (x, y, p)) {
					pattern = p;
					return true;
				}
			}
		}
		pattern = MatchPattern.Empty;
		return false;
	}

	public int FindAllMatchingTurns ()
	{
		int turns = 0;
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				if (TryFindMatchingTurns (i, j, out MatchPattern.Empty))
					turns++;
			}
		}
		return turns;
	}



	public List<Gem> highlightedGems = new List<Gem> ();
	public bool isHighlighted;

	public void HighlightMatchingTurns (int number)
	{
		if (!isHighlighted) {
			isHighlighted = true;
			highlightedGems = new List<Gem> ();
			MatchPattern p = MatchPattern.Empty;
			for (int i = 0; i < Size; i++) {
				for (int j = 0; j < Size; j++) {
					if (TryFindMatchingTurns (i, j, out p)) {
						highlightedGems.Add (board [i, j]);
						highlightedGems.Add (board [p.offset1.x + i, p.offset1.y + j]);
						highlightedGems.Add (board [p.offset2.x + i, p.offset2.y + j]);
					}
				}
			}

			while (number > 0) {
				if (highlightedGems.Count > 0) {
					int random = Random.Range (0, highlightedGems.Count / 3);
					highlightedGems [random * 3].Throb ();
					highlightedGems [random * 3 + 1].Throb ();
					highlightedGems [random * 3 + 2].Throb ();
				}
				number--;
			}
		}
	}

	public void HighlightAllMatchingTurns ()
	{
		if (!isHighlighted) {
			isHighlighted = true;
			highlightedGems = new List<Gem> ();
			MatchPattern p = MatchPattern.Empty;
			for (int i = 0; i < Size; i++) {
				for (int j = 0; j < Size; j++) {
					if (TryFindMatchingTurns (i, j, out p)) {
						highlightedGems.Add (board [i, j]);
						highlightedGems.Add (board [p.offset1.x + i, p.offset1.y + j]);
						highlightedGems.Add (board [p.offset2.x + i, p.offset2.y + j]);
					}
				}
			}
			foreach (var o in highlightedGems) {
				o.Throb ();
			}
		}
	}

	public void DehighlightAllMatcingTurns ()
	{
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				if (highlightedGems.Contains (board [i, j])) {
					if (boardData [i, j] > 0) {
						board [i, j].StopThrobbing ();
					}
				}
			}
		}
		isHighlighted = false;
	}

	int turn = 0;

	public void MakeTurn ()
	{
		MatchPattern p = MatchPattern.Empty;
		for (int i = 0; i < Size; i++) {
			for (int j = 0; j < Size; j++) {
				if (TryFindMatchingTurns (i, j, out p)) {
					turn++;
					Swap (new Vector2i (i + p.offset2.x, j + p.offset2.y), new Vector2i (i + p.offset3.x, j + p.offset3.y));
					return;
				}
			}
		}
		//Debug.Log ("NO MORE TURNS: already made: " + turn + " turns");
		//Application.LoadLevel (0);
	}
}
