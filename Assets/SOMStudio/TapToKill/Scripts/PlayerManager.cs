using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[AddComponentMenu("SOM Studio/Tap-To-Kill/Player Manager")]
public class PlayerManager : BasePlayerManager
{
	public string gamePrefsName = "DefaultGame";
	
	private readonly UnityEvent serverSuccessConnect = new UnityEvent();
	private readonly UnityEvent serverFailConnect = new UnityEvent();

	private bool needSavePlayerPrefs;

	[System.NonSerialized] public static PlayerManager Instance;

	private GameController gameController;
	
	private void Start()
	{
		StartInit();

		if (!gameController)
		{
			gameController = GameController.Instance;
		}
	}

	#region MainLogic
	protected override void Init()
	{
		DontDestroyOnLoad(this.gameObject);

		base.Init();
		
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
		dataManager.SetName("Player");
		
		serverSuccessConnect.AddListener(SuccessConnectServet);
		serverFailConnect.AddListener(FailConnectServet);
	}

	public override void GameStart()
	{
		base.GameStart();
		
		dataManager.SetScore(0);
		dataManager.SetHighScore(RestoreHighScore());
	}

	public override void GameFinished()
	{
		base.GameFinished();
		
		if (needSavePlayerPrefs)
		{
			dataManager.SetHighScore(dataManager.GetScore());
			
			SaveHighScore();

			needSavePlayerPrefs = false;
		}
	}

	public void AddScore(int val)
	{
		dataManager.AddScore(val);
		
		int score = dataManager.GetScore();
		int highScore = dataManager.GetHighScore();
		
		if (!needSavePlayerPrefs && score > highScore)
		{
			needSavePlayerPrefs = true;
			
			gameController.WindowAdwiceShowText("[c=blue]Congratulations, you have a new record![c]");
		}
	}

	public int GetScore()
	{
		return dataManager.GetScore();
	}

	public int GetHighScore()
	{
		return dataManager.GetHighScore();
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

	private void SuccessConnectServet()
	{
		gameController.SuccessConnectToServer();
	}

	private void FailConnectServet()
	{
		gameController.FailConnectToServer();
	}
	#endregion
}
