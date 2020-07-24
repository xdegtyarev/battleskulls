using UnityEngine;
public class BouncyAttack : Attack {
	public static BouncyAttack Instance;
	public Vector4 bounds; //x/y/w/h
	public Vector3 direction;
	public GameObject bouncyPrefab;
	public GameObject bouncy;
	public Transform bouncyTransform;
	public float speed;
	public override void Awake()
	{
		base.Awake ();
		if (isActive) {
			Instance = this;
			bouncy = Instantiate (bouncyPrefab, new Vector3 ((bounds.x + bounds.z) * 0.5f, (bounds.y + bounds.w) * 0.5f, -1f), Quaternion.identity) as GameObject;
			bouncyTransform = bouncy.transform;
			direction = new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), 0);
		}
	}

	public void Update(){
		if (isActive) {
			if (bouncyTransform.position.x < bounds.x) {
				direction = new Vector3 (-direction.x, direction.y, direction.z);
			} else if (bouncyTransform.position.y < bounds.y) {
				direction = new Vector3 (direction.x, -direction.y, direction.z);

			} else if (bouncyTransform.position.x > bounds.x + bounds.z) {
				direction = new Vector3 (-direction.x, direction.y, direction.z);
			} else if (bouncyTransform.position.y > bounds.y + bounds.w) {
				direction = new Vector3 (direction.x, -direction.y, direction.z);
			}
			bouncyTransform.position += direction * speed * Time.deltaTime;
		}
	}
}
