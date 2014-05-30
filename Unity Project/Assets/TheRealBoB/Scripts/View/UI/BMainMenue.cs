using UnityEngine;
using System.Collections;

public class BMainMenue : MonoBehaviour {

	public GameObject mainMenueCreditsPanel;
    public GameObject prototypePanel;
	public GameObject levelSelectionPanel;

	public void OnStartGameClick() 
	{
		gameObject.SetActive(false);
		levelSelectionPanel.SetActive(true);
	}

	public void OnCreditsClick() 
	{
		gameObject.SetActive(false);
		mainMenueCreditsPanel.SetActive(true);
	}

    public void OnPrototypeClick()
    {
        gameObject.SetActive(false);
        prototypePanel.SetActive(true);
    }

	public void OnExitGameClick() 
	{
		Application.Quit();
	}
}
