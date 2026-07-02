using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseGameController : MonoBehaviour
{
	private bool paused;
	
	public virtual void StartGame()
	{
	}

	public virtual void ExitGame()
	{
	}

	public virtual void RestartGameButtonPressed()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public bool Paused
	{
		get => paused;
		set
		{
			paused = value;

			if (paused)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}
}
