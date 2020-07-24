public class ThiefAttack : Attack {
	public static ThiefAttack Instance;
	public override void Awake(){
		base.Awake ();
		Instance = this;
	}
}
