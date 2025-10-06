using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void LaunchGame()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
