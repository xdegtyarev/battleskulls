	using UnityEngine;
using System.Collections;

public enum GemType{
	general
	//,
	//special,
	//specialVoid
}

public class Gem : MonoBehaviour {
	//Data
	public int id;
	public GemType type;
	public GameObject view;
	public Animator viewAnimator;
	//States
	public bool isMoving;
	public bool isFalling;
	public bool isThrobbing;
	//TweeningData;
	public float startFallSpeed = 10f;
	public float fallSpeed = 10f;
	public float gravity = 1f;
	public float swapSpeed = 10f;
	public float targetHeight;
	public float startHeight;
	public float progress;

	Transform t;

	void Awake()
	{
		t = transform;
	}
	
	void Register()
	{
		Board.Instance.Register(this);
	}
	
	void Deregister(int value)
	{

		Board.Instance.Deregister(new Vector2i(transform.position),value);
	}

	public void Init(GemType _type,int _id)
	{
		type = GemType.general;
		id = _id;
		view = Instantiate(GemFactory.instance.GetView(type,id),transform.position,Quaternion.identity) as GameObject;
		view.transform.parent = transform;
		view.transform.localPosition = Vector3.zero;
		viewAnimator = view.GetComponent<Animator>();
	}

	bool delayed;

	public IEnumerator SetFallingValue(int newTargetHeight,float delay)
	{

		if (newTargetHeight > Board.Size) {
			Debug.Break ();
			Debug.LogError ("HIGHER THAN BOARD!" +  newTargetHeight + " :" + transform.position.x);
			Debug.Log ("is alreadyFalling + " + isFalling + "targ height" + targetHeight);
		}
		StopThrobbing ();

		if (isFalling) {
			targetHeight = newTargetHeight;
			progress = (startHeight - t.position.y)/(startHeight-targetHeight);
		} else {
			if(transform.position.y<Board.Size){
				Deregister(-1);
			}
			fallSpeed = startFallSpeed;
			progress = 0f;
			isMoving = true;
			isFalling = true;
			startHeight = t.position.y;
			targetHeight = newTargetHeight;
		}

		delayed = true;
		yield return new WaitForSeconds(delay);
		delayed = false;
	}

	void Update()
	{
		if (isFalling) {
			if(!delayed){
				fallSpeed += gravity * Time.deltaTime;
				if (SpeedBoost.Instance.isSpeeded) {
					progress = progress + Time.deltaTime * SpeedBoost.Instance.speedValue / (startHeight - targetHeight);
				} else if (SlowAttack.Instance.isSlowed) {
					progress = progress + Time.deltaTime * SlowAttack.Instance.slowValue / (startHeight - targetHeight);
				}
				else{
					progress = progress + Time.deltaTime * fallSpeed / (startHeight - targetHeight);
				}
				t.position = new Vector3(t.position.x,Mathf.Lerp(startHeight,targetHeight,progress),0);

				if(progress >= 1f)
				{
					FallComplete();
				}
			}
		}
	}

	public void FallComplete()
	{
		t.position = new Vector3 (t.position.x, targetHeight, 0);
		progress = 0;
		isFalling = false;
		isMoving = false;
		Register ();
		Board.Instance.LogFallingGem (new Vector2i(transform.position));
	}

	public void Move (Vector2i trg) {
		StopThrobbing ();
		isMoving = true;
		iTween.MoveTo (gameObject, iTween.Hash("position", new Vector3 (trg.x, trg.y, 0), "speed", swapSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "MoveComplete", "oncompletetarget",gameObject));
	}

	public void MoveComplete()
	{
		isMoving = false;
		Register ();
		Board.Instance.OnSwapComplete (new Vector2i(transform.position));
	}

	public void Pop()
	{
//		if (id != GemFactory.ThiefItemId) {
//			if (type != GemType.specialVoid) {
//				Game.instance.currentProfile.AddScorePoints (id - 1, 1);//id-1 cause 0 is used in board data to display emptyness
//			}
//		}
		StopThrobbing ();
		Board.Instance.highlightedGems.Remove(this);
		ActivateAbility();
		StartCoroutine(DestroySelf());
	}

	public void ActivateAbility()
	{
//		if(type == GemType.special)
//		{
//			type = GemType.general;
//			AbilityManager.AddAbilityToLaunch(id,new Vector2i(transform.position));
//		}
	}

	public IEnumerator DestroySelf()
	{
		viewAnimator.SetTrigger("isDead");
		Deregister (0);
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

//	public IEnumerator UpgradeToSpecial()
//	{
//		viewAnimator.SetTrigger("isDead");
//		yield return new WaitForSeconds(0.5f);
//		Destroy(view);
//		Init(GemType.general,1);
////		Init(GemType.special,id);
//	}
//
//	public IEnumerator UpgradeToSpecialVoid ()
//	{
//		viewAnimator.SetTrigger("isDead");
//		yield return new WaitForSeconds(0.5f);
//		Destroy(view);
////		Init(GemType.specialVoid,GemFactory.SpecialVoidId);
//		Init(GemType.general,1);
//	}

	public void Throb()
	{
		isThrobbing = true;
//		animation ["ThrobGem"].wrapMode = WrapMode.Loop;
//		animation.Play ("ThrobGem");
	}

	public void StopThrobbing()
	{
		isThrobbing = false;
		if(animation != null)
			animation ["ThrobGem"].wrapMode = WrapMode.Once;
	}
}