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
		gameObject.active = false;
		mainMenueCreditsPanel.gameObject.active = true;
	}

	public void OnExitGameClick() 
	{
		Application.Quit();
	}
}
