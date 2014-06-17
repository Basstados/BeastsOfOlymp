using UnityEngine;
using System.Collections;

public class BMainMenue : MonoBehaviour {

	public GameObject mainMenueCreditsPanel;
    public GameObject prototypePanel;
	public GameObject levelSelectionPanel;
	public GameObject tutorialPanel;
	public GameObject tutorialBewegungsPanel;
	public GameObject kameraPanel;


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

	public void OnKameraClick()
	{
		gameObject.SetActive(false);
		kameraPanel.SetActive(true);
	}

	public void OnTutorialClick()
	{
		gameObject.SetActive(false);
		tutorialPanel.SetActive(true);
	}

	public void OnTutorialBewegungClick()
	{
		tutorialPanel.gameObject.SetActive(false);
		tutorialBewegungsPanel.SetActive(true);
	}

	public void OnExitGameClick() 
	{
		Application.Quit();
	}
}
