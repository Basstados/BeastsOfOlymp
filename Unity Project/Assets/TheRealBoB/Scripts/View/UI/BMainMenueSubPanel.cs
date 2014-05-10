using UnityEngine;
using System.Collections;


public class BMainMenueSubPanel : MonoBehaviour {

	public GameObject mainMenuePanel;

	public void OnBackButtonClick() 
	{
		mainMenuePanel.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}


}
