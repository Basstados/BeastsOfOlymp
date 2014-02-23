using UnityEngine;
using System.Collections;


public class BMainMenueCredits : MonoBehaviour {

	public GameObject mainMenuePanel;

	public void OnBackButtonClick() 
	{
		mainMenuePanel.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}


}
