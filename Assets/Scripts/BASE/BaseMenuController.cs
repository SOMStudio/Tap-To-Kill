using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseMenuController : MonoBehaviour
{
	public bool didInit = false;

	[Header("Base Settings")] public string gamePrefsName = "DefaultGame";
	
	public Slider audioSFXSlider;
	[SerializeField] private float audioSFXSliderValue;
	public Slider audioMusicSlider;
	[SerializeField] private float audioMusicSliderValue;

	private bool needSaveOptions = false;

	[Header("Main window list")] public AnimationOpenClose[] windowAnimations;

	[Header("DisActivate window")] public AnimationOpenClose windowDisActivateAnimation;
	public AnimationOpenClose consoleWindowDisActivateAnimation;

	[Header("Menu Data")] [SerializeField] protected int windowActive = -1;
	[SerializeField] protected int consoleWindowActive = -1;

	[Header("StartGame window")] [SerializeField]
	protected bool windowStartActive = false;

	public AnimationOpenClose windowStartGameAnimation;

	[Header("Advice window")] public AnimationOpenClose windowAdviceAnimation;
	public Text windowAdviceText;

	[Header("Inform window")] public AnimationOpenClose windowInformAnimation;
	public Text[] windowInformTextList;

	[Header("Console windows")]
	public Text consoleWInYesNoTextHead;

	private UnityEvent consoleWindowYesNoActionYes = new UnityEvent();
	
	protected virtual void Start()
	{
		RestoreOptionsPref();

		if (windowStartActive)
		{
			ShowWindowStartGame();
		}
	}

	#region MainLogic
	protected virtual void RestoreOptionsPref()
	{
		string stKey = "";

		// set up default options, if they have been saved out to prefs already
		stKey = $"{gamePrefsName}_SFXVol";
		if (PlayerPrefs.HasKey(stKey))
		{
			audioSFXSliderValue = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			// if we are missing an SFXVol key, we won't got audio defaults set up
			audioSFXSliderValue = 1;
		}

		stKey = $"{gamePrefsName}_MusicVol";
		if (PlayerPrefs.HasKey(stKey))
		{
			audioMusicSliderValue = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			// defaults set up
			audioMusicSliderValue = 1;
		}

		//set in UI
		if (audioSFXSlider != null)
		{
			audioSFXSlider.value = audioSFXSliderValue;
		}

		if (audioMusicSlider != null)
		{
			audioMusicSlider.value = audioMusicSliderValue;
		}

		windowAdviceText.text = "";

		didInit = true;
	}

	protected virtual void SaveOptionsPrefs()
	{
		string stKey = "";

		stKey = $"{gamePrefsName}_SFXVol";
		PlayerPrefs.SetFloat(stKey, audioSFXSliderValue);
		stKey = $"{gamePrefsName}_MusicVol";
		PlayerPrefs.SetFloat(stKey, audioMusicSliderValue);
	}

	public void ChangeSFXVal(float val)
	{
		audioSFXSliderValue = val;

		if (didInit)
		{
			SaveOptionsPrefs();
		}
	}

	public void ChangeMusicVal(float val)
	{
		audioMusicSliderValue = val;

		if (didInit)
		{
			SaveOptionsPrefs();
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
	private void PlayWindowAnim_Open(int number)
	{
		if (number < windowAnimations.Length)
		{
			AnimationOpenClose activeA = windowAnimations[number];

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();
				}
			}
		}
	}

	private void PlayWindowAnim_Close(int number)
	{
		if (number < windowAnimations.Length)
		{
			AnimationOpenClose activeA = windowAnimations[number];

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();
				}
			}
		}
	}
	
	private void WindowDisActivate_Open()
	{
		if (windowDisActivateAnimation)
		{
			windowDisActivateAnimation.Open();
		}
	}

	private void WindowDisActivate_Close()
	{
		if (windowDisActivateAnimation)
		{
			windowDisActivateAnimation.Close();
		}
	}
	
	private void ConsoleWindowDisActivate_Open()
	{
		if (consoleWindowDisActivateAnimation)
		{
			consoleWindowDisActivateAnimation.Open();
		}
	}

	private void ConsoleWindowDisActivate_Close()
	{
		if (consoleWindowDisActivateAnimation)
		{
			consoleWindowDisActivateAnimation.Close();
		}
	}
	
	private void PlayWindowStartGameAnim_Open()
	{
		if (windowStartGameAnimation)
		{
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();
				}
			}
		}
	}

	private void PlayWindowStartGameAnim_Close()
	{
		if (windowStartGameAnimation)
		{
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();
				}
			}
		}
	}
	
	private void PlayWindowAdviceAnim_Open()
	{
		if (windowAdviceAnimation)
		{
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();

					ActivateAdviceWEvent();
				}
			}
		}
	}

	private void PlayWindowAdviceAnim_Close()
	{
		if (windowAdviceAnimation)
		{
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();

					DisActivateAdviceWEvent();

					Invoke(nameof(WindowAdviceClearText), 0.2f);
				}
			}
		}
	}
	
	private void PlayWindowInformAnim_Open()
	{
		if (windowInformAnimation)
		{
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();

					ActivateInformWEvent();
				}
			}
		}
	}

	private void PlayWindowInformAnim_Close()
	{
		if (windowInformAnimation)
		{
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();

					DisActivateInformWEvent();
				}
			}
		}
	}
	#endregion

	#region PredefineEvents
	protected virtual void ActivateMenuEvent()
	{

	}

	protected virtual void DisActivateMenuEvent()
	{

	}

	protected virtual void ChangeMenuEvent(int number)
	{

	}

	protected virtual void ActivateWindowEvent()
	{

	}

	protected virtual void DisActivateWindowEvent()
	{

	}

	protected virtual void ChangeWindowEvent(int number)
	{

	}

	protected virtual void ActivateConsoleWEvent()
	{

	}

	protected virtual void DisActivateConsoleWEvent()
	{

	}

	protected virtual void ChangeConsoleWEvent(int number)
	{

	}

	protected virtual void ActivateAdviceWEvent()
	{

	}

	protected virtual void DisActivateAdviceWEvent()
	{

	}

	protected virtual void ActivateInformWEvent()
	{

	}

	protected virtual void DisActivateInformWEvent()
	{

	}
	#endregion

	#region WindowLogic
	protected void ActivateWindow(int number)
	{
		if (windowActive == number)
		{
			DisActivateWindow();
		}
		else
		{
			if (windowActive > -1)
			{
				PlayWindowAnim_Close(windowActive);
			}

			PlayWindowAnim_Open(number);

			if (windowActive == -1)
			{
				WindowDisActivate_Open();
				
				ActivateWindowEvent();
			}

			windowActive = number;
			
			ChangeWindowEvent(number);
		}
	}

	protected void DisActivateWindow()
	{
		if (windowActive > -1)
		{
			PlayWindowAnim_Close(windowActive);
			
			DisActivateWindowEvent();
		}

		windowActive = -1;

		WindowDisActivate_Close();

		//have some change options save it
		if (needSaveOptions)
		{
			SaveOptionsPrefs();

			needSaveOptions = !needSaveOptions;
		}
	}
	
	protected void ActivateConsoleWindow(int number)
	{
		if (consoleWindowActive == number)
		{
			DisActivateConsoleWindow();
		}
		else
		{
			if (consoleWindowActive > -1)
			{
				PlayWindowAnim_Close(consoleWindowActive);
			}

			PlayWindowAnim_Open(number);

			if (consoleWindowActive == -1)
			{
				ConsoleWindowDisActivate_Open();
				
				ActivateConsoleWEvent();
			}

			consoleWindowActive = number;
			
			ChangeConsoleWEvent(number);
		}
	}

	protected void DisActivateConsoleWindow()
	{
		if (consoleWindowActive > -1)
		{
			PlayWindowAnim_Close(consoleWindowActive);
		}

		consoleWindowActive = -1;

		ConsoleWindowDisActivate_Close();
		
		DisActivateConsoleWEvent();

		//have some change options save it
		if (needSaveOptions)
		{
			SaveOptionsPrefs();

			needSaveOptions = !needSaveOptions;
		}
	}
	
	public void ShowWindowStartGame()
	{
		PlayWindowStartGameAnim_Open();
		windowStartActive = true;

		if (IsInvoking(nameof(PlayWindowStartGameAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowStartGameAnim_Close));
		}
	}

	public void HideWindowStartGame()
	{
		if (IsInvoking(nameof(PlayWindowStartGameAnim_Close)))
		{

		}
		else
		{
			PlayWindowStartGameAnim_Close();
			windowStartActive = false;
		}
	}
	
	protected void ShowWindowAdvice()
	{
		PlayWindowAdviceAnim_Open();

		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowAdviceAnim_Close));
		}
	}

	protected void HideWindowAdvice()
	{
		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{

		}
		else
		{
			PlayWindowAdviceAnim_Close();
		}
	}

	protected void ShowWindowAdviceAtTime(float timeShow)
	{
		PlayWindowAdviceAnim_Open();

		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowAdviceAnim_Close));
		}

		Invoke(nameof(PlayWindowAdviceAnim_Close), timeShow);
	}

	protected void WindowAdviceSetText(string stAdvice)
	{
		if (windowAdviceText)
		{
			string stText = windowAdviceText.text;
			string[] stRes = stText.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

			if (stRes.Length > 2)
			{
				for (int i = 1; i < stRes.Length; i++)
				{
					if (i == 1)
					{
						windowAdviceText.text = stRes[i];
					}
					else
					{
						windowAdviceText.text = $"{windowAdviceText.text}\n{stRes[i]}";
					}
				}
			}

			if (stRes.Length == 0)
			{
				windowAdviceText.text = ConvertSpecTextChar(stAdvice);
			}
			else
			{
				windowAdviceText.text = $"{windowAdviceText.text}\n{ConvertSpecTextChar(stAdvice)}";
			}
		}
	}

	protected void WindowAdviceClearText()
	{
		windowAdviceText.text = "";
	}
	
	public void ShowWindowInform()
	{
		PlayWindowInformAnim_Open();
	}

	public void HideWindowInform()
	{
		PlayWindowInformAnim_Close();
	}

	protected void WindowInformSetText(string stAdvice, int numText)
	{
		if (numText < windowInformTextList.Length)
		{
			if (windowInformTextList[numText])
			{
				windowInformTextList[numText].text = ConvertSpecTextChar(stAdvice);
			}
		}
	}

	protected void WindowInformSetText_1(string stAdvice)
	{
		WindowInformSetText(stAdvice, 0);
	}

	protected void WindowInformSetText_2(string stAdvice)
	{
		WindowInformSetText(stAdvice, 1);
	}

	protected void WindowInformSetText_3(string stAdvice)
	{
		WindowInformSetText(stAdvice, 2);
	}
	
	protected void ConsoleWinYesNo_SetTxt(string val)
	{
		consoleWInYesNoTextHead.text = ConvertSpecTextChar(val);
	}

	protected void ConsoleWinYesNo_SetYesAction(UnityAction val)
	{
		consoleWindowYesNoActionYes.AddListener(val);
	}

	protected void ConsoleWinYesNo_ClearYesAction()
	{
		consoleWindowYesNoActionYes.RemoveAllListeners();
	}

	protected void ConsoleWinYesNo_ButtonYes()
	{
		consoleWindowYesNoActionYes.Invoke();

		DisActivateConsoleWindow();

		ConsoleWinYesNo_ClearYesAction();
	}

	protected void ConsoleWinYesNo_ButtonNo()
	{
		DisActivateConsoleWindow();

		ConsoleWinYesNo_ClearYesAction();
	}
	
	protected virtual bool HasSpecKeyText(string st)
	{
		return false;
	}

	protected virtual string ConvertSpecKeyText(string st)
	{
		if (HasSpecKeyText(st))
		{
			// in this override place set convert
		}

		return st;
	}

	protected string ConvertSpecTextChar(string st)
	{
		if (HasSpecKeyText(st))
		{
			st = ConvertSpecKeyText(st);
		}
		
		if (st.IndexOf("[c=", StringComparison.Ordinal) >= 0)
		{
			st = st.Replace("[c=red]", "<color=red>").Replace("[c=blue]", "<color=blue>")
				.Replace("[c=green]", "<color=green>").Replace("[c]", "</color>");
		}

		return st.Replace("[n]", "\n").Replace("[t]", "\t");
	}
	#endregion
}
