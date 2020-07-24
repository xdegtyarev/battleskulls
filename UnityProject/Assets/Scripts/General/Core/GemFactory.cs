using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class GemViewInspectorInfo{
	public int id;
	public GemType type;
	public GameObject view;
}

public class GemFactory : MonoBehaviour {
	public GameObject GemPrefab;
	public GemViewInspectorInfo[] views;
	public static GemFactory instance;
	public static int SpecialVoidId = 9;
	public static int ThiefItemId = 8;
	public int MaxGemIndex = 8;

	public void Awake()
	{
		instance = this;
	}

	void Start(){
		Debug.Log ("Thief enabled " + ThiefAttack.Instance.isActive);
		if(ThiefAttack.Instance.isActive){
			MaxGemIndex++;
		}
	}

	public GameObject GetView(GemType type, int id)
	{
		foreach(var o in views)
		{
			if(o.id == id && o.type == type)
				return o.view;
		}
		return null;
	}

	public List<Gem> SpawnGems(int column,int quantity)
	{
		List<Gem> gems = new List<Gem> ();
		List<Gem> currentChunk = Board.Instance.columns [column];
		float spawnHeight = 0;
		int lastColor = -1;
		int currentColor = -1;

		if (currentChunk.Count > 0){
			spawnHeight = currentChunk[currentChunk.Count-1].transform.position.y + 1;
			lastColor = currentChunk[currentChunk.Count-1].id;
		}
		else
		{
			spawnHeight = Board.Size;
		}

		for(int i = 0;i<quantity;i++)
		{

			do{
				currentColor = Random.Range (1,MaxGemIndex);
			}while(currentColor == lastColor);
				
			lastColor = currentColor;
			gems.Add(SpawnGem(column,spawnHeight + i,currentColor));
		}
		return gems;
	}

	public Gem SpawnGem(int x,float y,int id)
	{
		var gem = (GameObject)Instantiate(GemPrefab,new Vector3(x,y,0),Quaternion.identity);
		Gem g = gem.GetComponent<Gem>();
		g.Init(GemType.general,id);
		g.transform.parent = transform;
		return g;
	}
}