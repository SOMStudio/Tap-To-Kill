using UnityEngine;

public class ClickManager_TapToKill : ExtendedCustomMonoBehaviour2D {

	[SerializeField]
	private int bonus = 10;

	[SerializeField]
	private int secondsToDestroy = 2;

	// main event
	void Start() {
		// init main parameters
		Init ();

		// set destroy with delay
		Destroy (myGO, secondsToDestroy);
	}

	// main logic
	void OnMouseDown() {
		GameController_TapToKill.Instance.AddBonus (bonus);

		Destroy (myGO);
	}
}
