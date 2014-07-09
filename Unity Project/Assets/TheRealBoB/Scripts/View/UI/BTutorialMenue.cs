using UnityEngine;
using System.Collections;

public class BTutorialMenue : MonoBehaviour {

	public GameObject tutorialPanel;
	public GameObject tutorialBewegungsPanel;
	public GameObject tutorialBewegungsScreen1;
	public GameObject tutorialBewegungsScreen2;
	public GameObject tutorialBewegungsScreen3;
	public GameObject tutorialAngriffsPanel;
	public GameObject tutorialAngriffsScreen1;
	public GameObject tutorialAngriffsScreen2;
	public GameObject tutorialIniPanel;
	public GameObject tutorialIniScreen1;
	public GameObject tutorialIniScreen2;
	public GameObject initiativeList;

	

	
	public void OnTutorialClick()
	{
		gameObject.SetActive(false);
		initiativeList.SetActive(false);
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
	
	public void OnTutorialAngriffClick()
	{
		tutorialPanel.gameObject.SetActive(false);
		tutorialAngriffsPanel.SetActive(true);
	}
	
	public void OnAngriffsScreen1VorClick()
	{
		tutorialAngriffsScreen1.gameObject.SetActive(false);
		tutorialAngriffsScreen2.SetActive(true);
	}
	
	public void OnAngriffsScreen2BackClick()
	{
		tutorialAngriffsScreen2.gameObject.SetActive(false);
		tutorialAngriffsScreen1.SetActive(true);
	}
	
	public void OnIniClick()
	{
		tutorialPanel.gameObject.SetActive(false);
		tutorialIniPanel.SetActive(true);
	}
	
	public void OnIniScreen1VorClick()
	{
		tutorialIniScreen1.gameObject.SetActive(false);
		tutorialIniScreen2.SetActive(true);
	}
	
	public void OnIniScreen2BackClick()
	{
		tutorialIniScreen2.gameObject.SetActive(false);
		tutorialIniScreen1.SetActive(true);
	}

}
