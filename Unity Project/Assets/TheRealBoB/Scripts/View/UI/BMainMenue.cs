using UnityEngine;
using System.Collections;

public class BMainMenue : MonoBehaviour {

	public GameObject mainMenueCreditsPanel;

	public void OnStartGameClick() 
	{
		Application.LoadLevel ("testScene");
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
