using UnityEngine;

public class GameController_TapToKill : BaseGameController {

	[Header("Spawn settings")]
	public GameObject[] spawnList;
	public Transform[] spawnClip;
	public Transform spawnParent;

	[SerializeField]
	private float timeBetweenSpawn = 2.0f;
	[SerializeField]
	private float timeFrequencySpawn = 10;
	[SerializeField]
	private float timeDecreaseStep = 0.2f;
	[SerializeField]
	private float timeLimitBetweenSpawn = 0.5f;

	[Header("State Game")]
	[SerializeField]
	private bool connectionGame;
	[SerializeField]
	private bool startGame;
	
	private float timePlay;
	private float betweaneSpawnTime = 2.0f;
	private float lastSpawnTime;
	private float lastFrequenceSpawnTime;

	private TimerClass theTimer;

	[System.NonSerialized]
	public static GameController_TapToKill Instance;
	
	[Header("Managers")]
	[SerializeField]
	private MenuManager_TapToKill menuManager;
	[SerializeField]
	private PlayerManager_TapToKill playerManager;
	[SerializeField]
	private BaseSoundController soundManager;
	[SerializeField]
	private BaseMusicController musicManager;

	private void Awake () {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	private void Start () {
		InitGame ();
		
		Update_UI ();
		
		StartGame ();
	}

	private void Update () {
		if (startGame && !Paused) {
			theTimer.UpdateTimer ();
			int curTime = theTimer.GetTime ();
			
			timePlay += Time.deltaTime;
			
			SpawnManager (timePlay);
			
			SpawnFrequentManager (curTime);
			
			UpdateTimer_UI ();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus) {
			if (startGame && !Paused) {
				PauseGame ();
			}
		} else {
			if (startGame && Paused) {
				PauseGame ();
			}
		}
	}

	#region MainLogic
	private void InitGame () {
		DontDestroyOnLoad (this.gameObject);
		
		if (!menuManager) {
			menuManager = MenuManager_TapToKill.Instance;
		}
		
		if (!playerManager) {
			playerManager = PlayerManager_TapToKill.Instance;
		}
		
		if (!soundManager) {
			soundManager = BaseSoundController.Instance;
		}
		
		if (!musicManager) {
			musicManager = BaseMusicController.Instance;
		}
		
		theTimer = ScriptableObject.CreateInstance<TimerClass>();
	}

	public override void StartGame() {
		if (connectionGame) {
			startGame = true;
			
			theTimer.StartTimer ();
			
			timePlay = 0.0f;
			lastSpawnTime = timePlay;
			betweaneSpawnTime = timeBetweenSpawn;
			lastFrequenceSpawnTime = theTimer.GetTime ();
			
			playerManager.GameStart ();
			
			menuManager.HideWindowStartGame();
			menuManager.ShowWindowInform ();
			
			Update_UI ();
			
			WindowAdwiceShowText ("Good luck![n] Click < (Esc) for pause game.");
		} else {
			WindowAdwiceShowText ("Please connect to Server!");
		}
	}

	public override void ExitGame ()
	{
		startGame = false;
		
		playerManager.GameFinished();
	}

	public override void RestartGameButtonPressed ()
	{
		playerManager.GameFinished();
		
		theTimer.ResetTimer();
		
		StartGame ();
	}

	public void PauseGame() {
		if (Paused) {
			Paused = false;

			theTimer.StartTimer ();
		} else {
			Paused = true;

			theTimer.StopTimer ();
		}
	}

	private void SpawnManager(float time) {
		if (time - lastSpawnTime > betweaneSpawnTime) {
			GameObject objDrop = GetRandomGameObject ();
			Vector3 posDrop = GetRandomPossition ();
			
			var newGameObject = SpawnController.Instance.Spawn (objDrop, posDrop, Quaternion.identity);
			newGameObject.parent = spawnParent;

			lastSpawnTime = time;
		}
	}

	private void SpawnFrequentManager(float time) {
		if (betweaneSpawnTime > timeLimitBetweenSpawn) {
			if (time - lastFrequenceSpawnTime > timeFrequencySpawn) {
				betweaneSpawnTime -= timeDecreaseStep;

				lastFrequenceSpawnTime = time;
			}
		}
	}

	private GameObject GetRandomGameObject() {
		return spawnList [Random.Range (0, spawnClip.Length)];
	}

	private Vector3 GetRandomPossition() {
		float posX = Random.Range (spawnClip [0].position.x, spawnClip [1].position.x);
		float posY = Random.Range (spawnClip [0].position.y, spawnClip [1].position.y);

		return new Vector3 (posX, posY, spawnClip[0].position.z);
	}
	#endregion

	#region PlayerManager
	public void AddBonus(int val) {
		playerManager.AddScore (val);
		
		Update_UI ();
		
		if (val > 0) {
			IncreaseScore_Sound ();
		} else {
			DecreaseScore_Sound ();
		}
	}

	public void ConnectToServer() {
		if (!connectionGame) {
			playerManager.ConnectServer ();

			menuManager.UpdateConectionText ("[c=blue]Connecting...[c]");
		}
	}

	public void SuccessConnectToServer() {
		connectionGame = true;

		menuManager.UpdateConectionText ("[c=green]Connected...[c]");
		WindowAdwiceShowText ("- Success connect to server!", 2);
	}

	public void FailConnectToServer() {
		connectionGame = false;

		menuManager.UpdateConectionText ("[c=red]Not Connected...[c]");
		WindowAdwiceShowText ("- Fail connect to server! Check connection to internet.");
	}
	#endregion

	#region MenuManager
	public void Update_UI() {
		UpdateScore_UI();
		UpdateTimer_UI();
	}

	private void UpdateScore_UI() {
		string res = $"{playerManager.GetScore()}\\{playerManager.GetHighScore()}";

		menuManager.UpdateScore (res);
	}

	private void UpdateTimer_UI() {
		menuManager.UpdateTimer (theTimer.GetFormattedTime ());
	}

	public void WindowAdwiceShowText(string textVal, int timeVal = 0) {
		menuManager.WindowAdwiceShowText (textVal, timeVal);
	}
	#endregion

	#region SoundManager
	private void UpdateSoundVolume() {
		soundManager.UpdateVolume ();
	}

	private void UpdateMusicVolume() {
		musicManager.UpdateVolume ();
	}

	public void UpdateVolume() {
		UpdateSoundVolume ();
		UpdateMusicVolume ();
	}

	public void IncreaseScore_Sound() {
		soundManager.PlaySoundByIndex (0, Vector3.zero);
	}

	public void DecreaseScore_Sound() {
		soundManager.PlaySoundByIndex (1, Vector3.zero);
	}

	public void OpenMenu_Sound() {
		soundManager.PlaySoundByIndex (2, Vector3.zero);
	}

	public void CloseMenu_Sound() {
		soundManager.PlaySoundByIndex (3, Vector3.zero);
	}
	#endregion
}
