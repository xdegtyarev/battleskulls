using UnityEngine;

public class TouchProcessor : MonoBehaviour
{
	public static TouchProcessor Instance;
	RaycastHit hitInfo;
	public Vector2i selected;
	public bool isSelected;
	void Awake(){
		Instance = this;
	}

	void OnEnable ()
	{
		TouchManager.TouchMoveEvent += HandleTouchMoveEvent;
		TouchManager.TouchBeganEvent += HandleTouchBeganEvent;
	}

	void HandleTouchMoveEvent (TouchInfo touch)
	{
		if (isSelected) {
			Vector2i target = new Vector2i ();
			switch (touch.GetTouchMoveDirection ()) {
			case TouchInfo.TouchMoveDirection.UP:
				target = new Vector2i (selected.x, selected.y + 1);
				break;
			case TouchInfo.TouchMoveDirection.RIGHT:
				target = new Vector2i (selected.x + 1, selected.y);
				break;
			case TouchInfo.TouchMoveDirection.DOWN:
				target = new Vector2i (selected.x, selected.y - 1);
				break;
			case TouchInfo.TouchMoveDirection.LEFT:
				target = new Vector2i (selected.x - 1, selected.y);
				break;
			case TouchInfo.TouchMoveDirection.OTHER:
				return;
			}
			if(Board.Instance.boardData[target.x,target.y] > 0){
				Board.Instance.Swap (selected, target);
				isSelected = false;
			}
			else{
				Deselect();
			}
		}
	}

	bool AreNeighbours(Vector2i src,Vector2i trg)
	{
		return ((trg.x == src.x + 1 && trg.y == src.y) ||
			(trg.x == src.x - 1 && trg.y == src.y) ||
			(trg.x == src.x && trg.y == src.y + 1) ||
			(trg.x == src.x && trg.y == src.y - 1)); 
	}

	void Select(Vector2i point)
	{
		var gem = Board.Instance.board [point.x, point.y];
		if (gem != null) {
			isSelected = true;
			selected = point;
			Board.Instance.board[selected.x,selected.y].Throb();
		}
	}

	void Deselect()
	{
		if (isSelected) {
			isSelected = false;
			if(Board.Instance.boardData[selected.x,selected.y]>0)
			{
				if(Board.Instance.board[selected.x,selected.y] != null){
					Board.Instance.board [selected.x, selected.y].StopThrobbing ();
				}
			}
		}
	}
	
	void HandleTouchBeganEvent (TouchInfo touch)
	{
		if (Physics.Raycast (Camera.main.ScreenPointToRay (touch.position), out hitInfo)) {
			var target = new Vector2i (hitInfo.transform.position);
			if(isSelected){
				if (AreNeighbours(target,selected)) {
					Board.Instance.Swap (selected, target);
					isSelected = false;
				} else if(target.x == selected.x && target.y == selected.y){
					Deselect();
				}else{
					Deselect();
					Select(target);
				}
			}else{
				Select(target);
			}
		}
	}

	void OnDisable ()
	{
		TouchManager.MoveUpEvent -= HandleTouchMoveEvent;
		TouchManager.TouchBeganEvent -= HandleTouchBeganEvent;
	}
}
