using UnityEngine;
using System.Collections;

public class PanelToggle : MonoBehaviour {

	public UIButton mapButton;
	public UIButton attacksButton;
	public UIButton unitsButton;

	public UIPanel mapPanel;
	public UIPanel attacksPanel;
	public UIPanel unitsPanel;

	public Color active;
	public Color inactive;

	public void OpenMap() 
	{
		mapPanel.gameObject.SetActive(true);
		attacksPanel.gameObject.SetActive(false);
		unitsPanel.gameObject.SetActive(false);

		mapButton.defaultColor = active;
		attacksButton.defaultColor = inactive;
		unitsButton.defaultColor = inactive;
	}

	public void OpenAttacks()
	{
		mapPanel.gameObject.SetActive(false);
		attacksPanel.gameObject.SetActive(true);
		unitsPanel.gameObject.SetActive(false);
		
		mapButton.defaultColor = inactive;
		attacksButton.defaultColor = active;
		unitsButton.defaultColor = inactive;
	}

	public void OpenUnits()
	{
		mapPanel.gameObject.SetActive(false);
		attacksPanel.gameObject.SetActive(false);
		unitsPanel.gameObject.SetActive(true);
		
		mapButton.defaultColor = inactive;
		attacksButton.defaultColor = inactive;
		unitsButton.defaultColor = active;
	}
}
