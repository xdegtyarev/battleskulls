using UnityEngine;
public class Attack: MonoBehaviour
{
	public bool isActive;
	public static Attack Instance;

	public virtual void Awake () {
		Instance = this;
		isActive = PlayerPrefs.GetInt(name) == 1;
	}
}


