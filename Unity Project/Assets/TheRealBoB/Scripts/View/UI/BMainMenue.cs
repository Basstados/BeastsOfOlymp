using UnityEngine;
using System.Collections;

public class BMainMenue : MonoBehaviour {

	public GameObject mainMenueCreditsPanel;
	public string gameSceneName;

	public void OnStartGameClick() 
	{
		Application.LoadLevel(gameSceneName);
	}

	public void OnCreditsClick() 
	{
		gameObject.SetActive(false);
		mainMenueCreditsPanel.gameObject.SetActive(true);
	}

	public void OnExitGameClick() 
	{
		Application.Quit();
	}
}
