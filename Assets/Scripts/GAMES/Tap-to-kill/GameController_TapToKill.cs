using UnityEngine;

public class GameController_TapToKill : BaseGameController {

	[Header("Spawn settings")]
	public GameObject[] spawnList;					// prefabs foe spawn
	public Transform[] spawnClip; 					// clip in spawn possition
	public Transform spawnParent; 					// parent for spawned gameObject

	[SerializeField]
	private float timeBetweaneSpawn = 2.0f; 		// time betweane spawn
	[SerializeField]
	private float timeFrequenceSpawn = 10; 			// time after decreace spawn time
	[SerializeField]
	private float timeDecreaceStep = 0.2f; 			// step decreace spawn
	[SerializeField]
	private float timeLimitBetweaneSpawn = 0.5f; 	// limit betweane spawn

	[Header("State Game")]
	[SerializeField]
	private bool connectionGame = false;
	[SerializeField]
	private bool startGame = false;

	// spawn settings
	private float timePlay = 0.0f;
	private float betweaneSpawnTime = 2.0f;
	private float lastSpawnTime = 0.0f;
	private float lastFrequenceSpawnTime = 0.0f;

	private TimerClass theTimer;

	[System.NonSerialized]
	public static GameController_TapToKill Instance;

	// its init from instance but better set in panel
	[Header("Managers")]
	[SerializeField]
	private MenuManager_TapToKill menuManager;
	[SerializeField]
	private PlayerManager_TapToKill playerManager;
	[SerializeField]
	private BaseSoundController soundManager;
	[SerializeField]
	private BaseMusicController musicManager;

	// main event
	void Awake () {
		// activate instance
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		// init game
		InitGame ();

		// crear Menu
		Update_UI ();

		// start game
		StartGame ();
	}

	void Update () {
		if (startGame && !Paused) {
			// global time for frequence spawn
			theTimer.UpdateTimer ();
			int curTime = theTimer.GetTime ();

			// time for Spawn
			timePlay += Time.deltaTime;

			// spawn Manager
			SpawnManager (timePlay);

			// delay increase spawn frequence
			SpawnFrequentManager (curTime);

			// update time Timer
			UpdateTimer_UI ();
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus) {
			//pause game
			if (startGame && !Paused) {
				PauseGame ();
			}
		} else {
			//continue game
			if (startGame && Paused) {
				PauseGame ();
			}
		}
	}

	#region MainLogic
	private void InitGame () {
		// keep this object alive
		DontDestroyOnLoad (this.gameObject);

		// init nemuManager ref
		if (!menuManager) {
			menuManager = MenuManager_TapToKill.Instance;
		}

		// chack playerManager ref
		if (!playerManager) {
			playerManager = PlayerManager_TapToKill.Instance;
		}

		// init soundManager
		if (!soundManager) {
			soundManager = BaseSoundController.Instance;
		}

		// init musicManager
		if (!musicManager) {
			musicManager = BaseMusicController.Instance;
		}

		// initialize a timer
		theTimer = ScriptableObject.CreateInstance<TimerClass>();
	}

	public override void StartGame() {
		if (connectionGame) {
			startGame = true;

			// start Timer
			theTimer.StartTimer ();

			// get time
			timePlay = 0.0f;
			lastSpawnTime = timePlay;
			betweaneSpawnTime = timeBetweaneSpawn;
			lastFrequenceSpawnTime = theTimer.GetTime ();

			// player manager
			playerManager.GameStart ();

			// init interface
			menuManager.HideWindowStartGame();
			menuManager.ShowWindowInform ();
			// update score UI
			Update_UI ();

			// show message in adwiceWindow
			WindowAdwiceShowText ("Good luck![n] Click < (Esc) for pouse game.");
		} else {
			WindowAdwiceShowText ("Please connect to Server!");
		}
	}

	public override void ExitGame ()
	{
		startGame = false;

		// player manager save progress
		playerManager.GameFinished();
	}

	public override void RestartGameButtonPressed ()
	{
		// player manager
		playerManager.GameFinished();

		// reset Timer
		theTimer.ResetTimer();

		// start game
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

			// instantiate? set parrent
			var newGO = SpawnController.Instance.Spawn (objDrop, posDrop, Quaternion.identity);
			newGO.parent = spawnParent;

			lastSpawnTime = time;
		}
	}

	private void SpawnFrequentManager(float time) {
		if (betweaneSpawnTime > timeLimitBetweaneSpawn) {
			if (time - lastFrequenceSpawnTime > timeFrequenceSpawn) {
				betweaneSpawnTime -= timeDecreaceStep;

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
		// add score in Data
		playerManager.AddScore (val);

		// update score UI
		Update_UI ();

		// play sound
		if (val > 0) {
			IncreaseScore_Sound ();
		} else {
			DecreaseScore_Sound ();
		}
	}

	public void ConnectToServer() {
		if (!connectionGame) {
			// try connect to Server
			playerManager.ConnectServer ();

			menuManager.UpdateConectionText ("[c=blue]Connecting...[c]");
		}
	}

	public void SaccesConnectToServer() {
		connectionGame = true;

		menuManager.UpdateConectionText ("[c=green]Connected...[c]");
		WindowAdwiceShowText ("- Sacces connect to server!", 2);
	}

	public void FailConnectToServer() {
		connectionGame = false;

		menuManager.UpdateConectionText ("[c=red]Not Connected...[c]");
		WindowAdwiceShowText ("- Fail connect to server! Chack connection to internet.");
	}
	#endregion

	#region MenuManager
	public void Update_UI() {
		UpdateScore_UI();
		UpdateTimer_UI();
	}

	private void UpdateScore_UI() {
		string res = string.Format ("{0}\\{1}", playerManager.GetScore (), playerManager.GetHighScore ());

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
		musicManager.UpdateValume ();
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
