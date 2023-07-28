using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseGameController : MonoBehaviour
{
	private bool paused;
	
	public virtual void StartGame()
	{
		// do start game functions
	}

	public virtual void ExitGame()
	{
		// do end game functions
	}

	public virtual void RestartGameButtonPressed()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public bool Paused
	{
		get
		{
			// get paused
			return paused;
		}
		set
		{
			// set paused 
			paused = value;

			if (paused)
			{
				// pause time
				Time.timeScale = 0f;
			}
			else
			{
				// unpause Unity
				Time.timeScale = 1f;
			}
		}
	}
}
