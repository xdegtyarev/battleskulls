using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Gem))]
public class GemEditor : Editor {
	Gem trg;
	public int id;
	public GemType type;

	public void OnEnable () {
		trg = target as Gem;
		id = trg.id;
	}
	
	// Update is called once per frame
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		id = (int)GUILayout.HorizontalSlider(id,0,8);
		if(GUILayout.Button("Create Basic"))
		{
			type = GemType.general;
			UpdateGemData();
		}
	}

	void UpdateGemData()
	{
		Destroy(trg.view);
		trg.Init(type,id);
		Vector2i v2i = new Vector2i(trg.transform.position);
		Board.Instance.boardData [v2i.x, v2i.y] = id;
	}
}
