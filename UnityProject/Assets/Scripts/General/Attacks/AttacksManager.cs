using UnityEngine;
public class AttacksManager: MonoBehaviour
{
	public static AttacksManager Instance;
	public Attack[] attacksBase;
	void Awake()
	{
		Instance = this;
	}
}

