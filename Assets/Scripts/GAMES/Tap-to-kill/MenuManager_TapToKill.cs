using UnityEngine;
using UnityEngine.UI;

public class MenuManager_TapToKill : BaseMenuController {
	[Header("TapToKill_Settings")]
	public Text connectionText;

	[System.NonSerialized]
	public static MenuManager_TapToKill Instance;

	private GameController_TapToKill gameController;

	private void Awake () {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	protected override void Start() {
		base.Start ();

		if (!gameController) {
			gameController = GameController_TapToKill.Instance;
		}
	}

	void LateUpdate() {
		if (gameController) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				gameController.PauseGame ();
			}
		}
	}

	#region PredefineEvents
	protected override void ChangeWindowEvent (int number)
	{
		gameController.OpenMenu_Sound ();
	}

	protected override void DisActivateWindowEvent ()
	{
		gameController.CloseMenu_Sound ();
	}

	protected override void ChangeConsoleWEvent (int number)
	{
		gameController.OpenMenu_Sound ();
	}

	protected override void DisActivateConsoleWEvent ()
	{
		gameController.CloseMenu_Sound ();
	}
	#endregion

	#region MainLogic
	protected override void ExitGame ()
	{
		SaveOptionsPrefs ();
		gameController.ExitGame ();
		
		base.ExitGame ();
	}

	private void ClickEscapeEvent() {
		if (windowStartActive) {
			if (windowActive == 0) {
				DisActivateWindow ();
			} else if (windowActive == 2) {
				DisActivateConsoleWindow ();
			}
		} else {
			if (windowActive == -1) {
				gameController.PauseGame ();
			}
		}
	}

	protected override void SaveOptionsPrefs ()
	{
		base.SaveOptionsPrefs ();

		gameController.UpdateVolume ();
	}

	public void UpdateScore(string val) {
		WindowInformSetText_2 (val);
	}

	public void UpdateTimer(string val) {
		WindowInformSetText_1 (val);
	}

	public void UpdateConectionText(string val) {
		connectionText.text = ConvertSpecTextChar (val);
	}

	public void WindowAdwiceShowText(string textVal, int timeVal = 0) {
		WindowAdviceSetText (textVal);
		if (timeVal != 0) {
			ShowWindowAdviceAtTime (timeVal);
		} else {
			if (textVal.Length <= 40) {
				ShowWindowAdviceAtTime (5);
			} else {
				ShowWindowAdviceAtTime (10);
			}
		}
	}
	#endregion

	#region buttonHandlers
	public void RestartGame_Button() {
		gameController.RestartGameButtonPressed ();
		
		gameController.OpenMenu_Sound ();
	}

	public void Conection_Button() {
		gameController.ConnectToServer ();
		
		gameController.OpenMenu_Sound ();
	}

	public void OpenWindowSettings_Button() {
		ActivateWindow (0);
	}

	public void OpenWindowStartGame_Button() {
		gameController.ExitGame ();

		HideWindowInform ();
		ShowWindowStartGame ();
		
		gameController.OpenMenu_Sound ();
	}

	public void ExitGame_Button() {
		ConsoleWinYesNo_SetTxt ("Exit fromGame?");
		ConsoleWinYesNo_SetYesAction (ExitGame);

		ActivateConsoleWindow (2);
	}
	#endregion
}
