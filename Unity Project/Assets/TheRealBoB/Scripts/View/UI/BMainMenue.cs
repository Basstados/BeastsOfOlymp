using UnityEngine;
using System.Collections;

public class BMainMenue : MonoBehaviour {

	public GameObject mainMenueCreditsPanel;
    public GameObject prototypePanel;
	public GameObject levelSelectionPanel;
	public GameObject tutorialPanel;
	public GameObject tutorialBewegungsPanel;
	public GameObject tutorialBewegungsScreen1;
	public GameObject tutorialBewegungsScreen2;
	public GameObject tutorialBewegungsScreen3;
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

	public void OnBewegungScreen1VorClick()
	{
		tutorialBewegungsScreen1.gameObject.SetActive(false);
		tutorialBewegungsScreen2.SetActive(true);
	}

	public void OnBewegungScreen2VorClick()
	{
		tutorialBewegungsScreen2.gameObject.SetActive(false);
		tutorialBewegungsScreen3.SetActive(true);
	}

	public void OnBewegungScreen2BackClick()
	{
		tutorialBewegungsScreen2.gameObject.SetActive(false);
		tutorialBewegungsScreen1.SetActive(true);
	}

	public void OnBewegungScreen3BackClick()
	{
		tutorialBewegungsScreen3.gameObject.SetActive(false);
		tutorialBewegungsScreen2.SetActive(true);
	}

	public void OnExitGameClick() 
	{
		Application.Quit();
	}
}
