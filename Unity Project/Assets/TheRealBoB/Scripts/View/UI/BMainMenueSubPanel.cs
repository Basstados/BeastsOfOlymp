using UnityEngine;
using System.Collections;


public class BMainMenueSubPanel : MonoBehaviour {

	public GameObject mainMenuePanel;
	public GameObject tutorialListPanel;

	public void OnBackButtonClick() 
	{
		mainMenuePanel.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnBackToTutListButtonClick() 
	{
		tutorialListPanel.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}


}
