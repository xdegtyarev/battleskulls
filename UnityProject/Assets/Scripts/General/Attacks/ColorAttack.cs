public class ColorAttack: Attack{
	public static ColorAttack Instance;
	public override void Awake(){
		base.Awake ();
		Instance = this;
	}
}

