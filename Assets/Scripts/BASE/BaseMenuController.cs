using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseMenuController : MonoBehaviour {

	public bool didInit = false;

	[Header("Base Settings")]
	public string gamePrefsName= "DefaultGame";
	//audio
	public Slider audioSFXSlider;
	[SerializeField]
	private float audioSFXSliderValue;
	public Slider audioMusicSlider;
	[SerializeField]
	private float audioMusicSliderValue;

	private bool needSaveOptions = false;

	[Header("Main window list")]
	public AnimationOpenClose[] windowAnimations;

	[Header("DisActivate window")]
	public AnimationOpenClose windowDisActivateAnimation;
	public AnimationOpenClose consoleWindowDisActivateAnimation;

	[Header("Menu Data")]
	[SerializeField]
	protected int windowActive = -1;
	[SerializeField]
	protected int consoleWindowActive = -1;

	[Header("StartGame window")]
	[SerializeField]
	protected bool windowStartActive = false;
	public AnimationOpenClose windowStartGameAnimation;

	[Header("Advice window")]
	public AnimationOpenClose windowAdviceAnimation;
	public Text windowAdviceText;

	[Header("Inform window")]
	public AnimationOpenClose windowInformAnimation;
	public Text[] windowInformTextList;

	[Header("Console windows")]
	//YesNo window
	public Text consoleWInYesNoTextHead;
	private UnityEvent consoleWInYesNoActinYes = new UnityEvent();

	// main event
	protected virtual void Start()
	{
		RestoreOptionsPref ();

		if (windowStartActive) {
			ShowWindowStartGame ();
		}
	}

	#region MainLogic
	protected virtual void RestoreOptionsPref()
	{
		string stKey = "";

		// set up default options, if they have been saved out to prefs already
		stKey = string.Format("{0}_SFXVol", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			audioSFXSliderValue = PlayerPrefs.GetFloat (stKey);
		} else {
			// if we are missing an SFXVol key, we won't got audio defaults set up
			audioSFXSliderValue = 1;
		}
		stKey = string.Format("{0}_MusicVol", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			audioMusicSliderValue = PlayerPrefs.GetFloat (stKey);
		} else {
			// defaults set up
			audioMusicSliderValue = 1;
		}

		//set in UI
		if (audioSFXSlider != null) {
			audioSFXSlider.value = audioSFXSliderValue;
		}
		if (audioMusicSlider != null) {
			audioMusicSlider.value = audioMusicSliderValue;
		}

		windowAdviceText.text = "";

		didInit = true;
	}

	protected virtual void SaveOptionsPrefs()
	{
		string stKey = "";

		stKey = string.Format("{0}_SFXVol", gamePrefsName);
		PlayerPrefs.SetFloat(stKey, audioSFXSliderValue);
		stKey = string.Format("{0}_MusicVol", gamePrefsName);
		PlayerPrefs.SetFloat(stKey, audioMusicSliderValue);
	}

	public void ChangeSFXVal(float val) {
		audioSFXSliderValue = val;

		if (didInit) {
			SaveOptionsPrefs ();
		}
	}

	public void ChangeMusicVal(float val) {
		audioMusicSliderValue = val;

		if (didInit) {
			SaveOptionsPrefs ();
		}
	}

	protected virtual void ExitGame()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	#endregion

	#region MenuAnimations
	//windows
	private void PlayWindowAnim_Open(int number) {
		if (number < windowAnimations.Length) {
			AnimationOpenClose activeA = windowAnimations [number];

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayWindowAnim_Close(int number) {
		if (number < windowAnimations.Length) {
			AnimationOpenClose activeA = windowAnimations [number];

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();
				}
			}
		}
	}

	//window disActivate
	private void WindowDisActivate_Open() {
		if (windowDisActivateAnimation) {
			windowDisActivateAnimation.Open ();
		}
	}

	private void WindowDisActivate_Close() {
		if (windowDisActivateAnimation) {
			windowDisActivateAnimation.Close ();
		}
	}

	//consoleWindows disActivate
	private void ConsoleWindowDisActivate_Open() {
		if (consoleWindowDisActivateAnimation) {
			consoleWindowDisActivateAnimation.Open ();
		}
	}

	private void ConsoleWindowDisActivate_Close() {
		if (consoleWindowDisActivateAnimation) {
			consoleWindowDisActivateAnimation.Close ();
		}
	}

	//startGame window
	private void PlayWindowStartGameAnim_Open() {
		if (windowStartGameAnimation) {
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayWindowStartGameAnim_Close() {
		if (windowStartGameAnimation) {
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();
				}
			}
		}
	}

	//advice window
	private void PlayWindowAdviceAnim_Open() {
		if (windowAdviceAnimation) {
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();

					ActivateAdviceWEvent ();
				}
			}
		}
	}

	private void PlayWindowAdviceAnim_Close() {
		if (windowAdviceAnimation) {
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();

					DisActivateAdviceWEvent ();

					Invoke ("WindowAdviceClearText", 0.2f);
				}
			}
		}
	}

	//inform window
	private void PlayWindowInformAnim_Open() {
		if (windowInformAnimation) {
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();

					ActivateInformWEvent ();
				}
			}
		}
	}

	private void PlayWindowInformAnim_Close() {
		if (windowInformAnimation) {
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();

					DisActivateInformWEvent ();
				}
			}
		}
	}
	#endregion

	#region PredefineEvents
	protected virtual void ActivateMenuEvent() {
		
	}

	protected virtual void DisActivateMenuEvent() {

	}

	protected virtual void ChancheMenuEvent(int number) {

	}

	protected virtual void ActivateWindowEvent() {

	}

	protected virtual void DisActivateWindowEvent() {

	}

	protected virtual void ChancheWindowEvent(int number) {

	}

	protected virtual void ActivateConsoleWEvent() {

	}

	protected virtual void DisActivateConsoleWEvent() {

	}

	protected virtual void ChancheConsoleWEvent(int number) {

	}

	protected virtual void ActivateAdviceWEvent() {

	}

	protected virtual void DisActivateAdviceWEvent() {

	}

	protected virtual void ActivateInformWEvent() {

	}

	protected virtual void DisActivateInformWEvent() {

	}
	#endregion

	#region WindowLogic
	//windows
	protected void ActivateWindow(int number) {
		if (windowActive == number) {
			DisActivateWindow ();
		} else {
			if (windowActive > -1) {
				PlayWindowAnim_Close (windowActive);
			}

			PlayWindowAnim_Open (number);

			if (windowActive == -1) {
				WindowDisActivate_Open ();

				//event
				ActivateWindowEvent ();
			}

			windowActive = number;

			//event
			ChancheWindowEvent (number);
		}
	}

	protected void DisActivateWindow() {
		if (windowActive > -1) {
			PlayWindowAnim_Close (windowActive);

			//event
			DisActivateWindowEvent ();
		}

		windowActive = -1;

		WindowDisActivate_Close ();

		//have some change options save it
		if (needSaveOptions) {
			SaveOptionsPrefs ();

			needSaveOptions = !needSaveOptions;
		}
	}

	//consoleWindows
	protected void ActivateConsoleWindow(int number) {
		if (consoleWindowActive == number) {
			DisActivateConsoleWindow ();
		} else {
			if (consoleWindowActive > -1) {
				PlayWindowAnim_Close (consoleWindowActive);
			}

			PlayWindowAnim_Open (number);

			if (consoleWindowActive == -1) {
				ConsoleWindowDisActivate_Open ();

				//event
				ActivateConsoleWEvent ();
			}

			consoleWindowActive = number;

			//event
			ChancheConsoleWEvent (number);
		}
	}

	protected void DisActivateConsoleWindow() {
		if (consoleWindowActive > -1) {
			PlayWindowAnim_Close (consoleWindowActive);
		}

		consoleWindowActive = -1;

		ConsoleWindowDisActivate_Close ();

		//event
		DisActivateConsoleWEvent ();

		//have some change options save it
		if (needSaveOptions) {
			SaveOptionsPrefs ();

			needSaveOptions = !needSaveOptions;
		}
	}

	//startGame
	public void ShowWindowStartGame() {
		PlayWindowStartGameAnim_Open ();
		windowStartActive = true;

		if (IsInvoking ("PlayWindowStartGameAnim_Close")) {
			CancelInvoke ("PlayWindowStartGameAnim_Close");
		}
	}

	public void HideWindowStartGame() {
		if (IsInvoking ("PlayWindowStartGameAnim_Close")) {

		} else {
			PlayWindowStartGameAnim_Close ();
			windowStartActive = false;
		}
	}

	//advice
	protected void ShowWindowAdvice() {
		PlayWindowAdviceAnim_Open ();

		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {
			CancelInvoke ("PlayWindowAdviceAnim_Close");
		}
	}

	protected void HideWindowAdvice() {
		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {

		} else {
			PlayWindowAdviceAnim_Close ();
		}
	}

	protected void ShowWindowAdviceAtTime(float timeShow) {
		PlayWindowAdviceAnim_Open ();

		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {
			CancelInvoke ("PlayWindowAdviceAnim_Close");
		}

		Invoke ("PlayWindowAdviceAnim_Close", timeShow);
	}

	protected void WindowAdviceSetText(string stAdvice) {
		if (windowAdviceText) {
			string stText = windowAdviceText.text;
			string[] stRes = stText.Split (new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

			if (stRes.Length > 2) {
				for (int i = 1; i < stRes.Length; i++) {
					if (i==1) {
						windowAdviceText.text = stRes [i];
					} else {
						windowAdviceText.text = string.Format ("{0}\n{1}", windowAdviceText.text, stRes [i]);
					}
				}
			}

			if (stRes.Length == 0) {
				windowAdviceText.text = ConvertSpecTextChar (stAdvice);
			} else {
				windowAdviceText.text = string.Format ("{0}\n{1}", windowAdviceText.text, ConvertSpecTextChar (stAdvice));
			}
		}
	}

	protected void WindowAdviceClearText() {
		windowAdviceText.text = "";
	}

	//inform
	public void ShowWindowInform() {
		PlayWindowInformAnim_Open ();
	}

	public void HideWindowInform() {
		PlayWindowInformAnim_Close ();
	}

	protected void WindowInformSetText(string stAdvice, int numText) {
		if (numText < windowInformTextList.Length) {
			if (windowInformTextList [numText]) {
				windowInformTextList [numText].text = ConvertSpecTextChar (stAdvice);
			}
		}
	}

	protected void WindowInformSetText_1(string stAdvice) {
		WindowInformSetText (stAdvice, 0);
	}

	protected void WindowInformSetText_2(string stAdvice) {
		WindowInformSetText (stAdvice, 1);
	}

	protected void WindowInformSetText_3(string stAdvice) {
		WindowInformSetText (stAdvice, 2);
	}

	//console YesNo
	protected void ConsoleWinYesNo_SetTxt(string val) {
		consoleWInYesNoTextHead.text = ConvertSpecTextChar(val);
	}

	protected void ConsoleWinYesNo_SetYesAction(UnityAction val) {
		consoleWInYesNoActinYes.AddListener (val);
	}

	protected void ConsoleWinYesNo_ClearYesAction() {
		consoleWInYesNoActinYes.RemoveAllListeners ();
	}

	protected void ConsoleWinYesNo_ButtonYes() {
		consoleWInYesNoActinYes.Invoke ();

		DisActivateConsoleWindow ();

		ConsoleWinYesNo_ClearYesAction ();
	}

	protected void ConsoleWinYesNo_ButtonNo() {
		DisActivateConsoleWindow ();

		ConsoleWinYesNo_ClearYesAction ();
	}

	//convert spec text
	protected virtual bool HasSpecKeyText(string st) {
		return false;
	}

	protected virtual string ConvertSpecKeyText(string st) {
		if (HasSpecKeyText(st)) {
			// in this override place set convert
		}

		return st;
	}

	protected string ConvertSpecTextChar(string st) {
		// text key
		if (HasSpecKeyText (st)) {
			st = ConvertSpecKeyText (st);
		}

		// text color
		if (st.IndexOf ("[c=") >= 0) {
			st = st.Replace ("[c=red]", "<color=red>").Replace ("[c=blue]", "<color=blue>").Replace ("[c=green]", "<color=green>").Replace ("[c]", "</color>");
		}

		return st.Replace ("[n]", "\n").Replace ("[t]", "\t");
	}
	#endregion
}
