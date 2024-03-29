﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerManager_TapToKill : BasePlayerManager
{
	public string gamePrefsName = "DefaultGame";
	
	private readonly UnityEvent serverSuccessConnect = new UnityEvent();
	private readonly UnityEvent serverFailConnect = new UnityEvent();

	private bool needSavePlayerPrefs;

	[System.NonSerialized] public static PlayerManager_TapToKill Instance;

	private GameController_TapToKill gameController;
	
	private void Start()
	{
		StartInit();

		if (!gameController)
		{
			gameController = GameController_TapToKill.Instance;
		}
	}

	#region MainLogic
	public override void Init()
	{
		// keep this object alive
		DontDestroyOnLoad(this.gameObject);

		base.Init();

		// init instance
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void StartInit()
	{
		// init Player Data
		DataManager.SetName("Player");

		// set events
		serverSuccessConnect.AddListener(SaccesConectServet);
		serverFailConnect.AddListener(FailConectServet);
	}

	public override void GameStart()
	{
		base.GameStart();

		// clear data
		DataManager.SetScore(0);
		DataManager.SetHighScore(RestoreHighScore());
	}

	public override void GameFinished()
	{
		base.GameFinished();

		// check save data
		if (needSavePlayerPrefs)
		{
			// update high score
			DataManager.SetHighScore(DataManager.GetScore());

			// save in pref
			SaveHighScore();

			needSavePlayerPrefs = false;
		}
	}

	public void AddScore(int val)
	{
		DataManager.AddScore(val);

		// check highScore
		int score = DataManager.GetScore();
		int highScore = DataManager.GetHighScore();
		
		if (!needSavePlayerPrefs && score > highScore)
		{
			needSavePlayerPrefs = true;

			// set message in AdviceWindow
			gameController.WindowAdwiceShowText("[c=blue]Congratulations, you have a new record![c]");
		}
	}

	public int GetScore()
	{
		return DataManager.GetScore();
	}

	public int GetHighScore()
	{
		return DataManager.GetHighScore();
	}
	#endregion

	#region PlayerPrefs
	protected int RestoreHighScore()
	{
		string stKey = $"{gamePrefsName}_HighScore";

		if (PlayerPrefs.HasKey(stKey))
		{
			return PlayerPrefs.GetInt(stKey);
		}
		else
		{
			return 0;
		}
	}

	protected void SaveHighScore()
	{
		string stKey = $"{gamePrefsName}_HighScore";

		PlayerPrefs.SetInt(stKey, GetHighScore());
	}
	#endregion

	#region ServetConnect
	private IEnumerator ConnectToServer()
	{

		int res = Random.Range(1, 11);

		yield return new WaitForSeconds(5f);

		if (res < 5)
		{
			serverSuccessConnect.Invoke();
		}
		else
		{
			serverFailConnect.Invoke();
		}
	}


	public void ConnectServer()
	{
		StartCoroutine(ConnectToServer());
	}

	public void DisconnectServer()
	{

	}

	private void SaccesConectServet()
	{
		gameController.SaccesConnectToServer();
	}

	private void FailConectServet()
	{
		gameController.FailConnectToServer();
	}

	#endregion
}
