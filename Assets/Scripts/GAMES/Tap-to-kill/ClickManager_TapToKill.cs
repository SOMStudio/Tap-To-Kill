using UnityEngine;

[AddComponentMenu("SOM Studio/Tap-To-Kill/Click Manager")]
public class ClickManager_TapToKill : ExtendedCustomMonoBehaviour2D
{
	[SerializeField] private int bonus = 10;
	[SerializeField] private int secondsToDestroy = 2;

	private void Start()
	{
		Init();
		
		Destroy(myGameObject, secondsToDestroy);
	}

	private void OnMouseDown()
	{
		GameController_TapToKill.Instance.AddBonus(bonus);

		Destroy(myGameObject);
	}
}
