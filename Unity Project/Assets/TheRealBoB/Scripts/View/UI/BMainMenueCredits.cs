using UnityEngine;
using System.Collections;


public class BMainMenueCredits : MonoBehaviour {

	public GameObject mainMenuePanel;

	public void OnBackButtonClick() 
	{
		mainMenuePanel.gameObject.active = true;
		gameObject.active = false;
	}


}
