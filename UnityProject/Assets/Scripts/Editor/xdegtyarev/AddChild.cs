using UnityEngine;
using UnityEditor;

public class AddChild : MonoBehaviour
{
	[MenuItem ("GameObject/Create Empty Child &#n")]
	static void EmptyChild ()
	{
		GameObject go = new GameObject ("Empty");
		go.transform.parent = Selection.activeTransform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
}
