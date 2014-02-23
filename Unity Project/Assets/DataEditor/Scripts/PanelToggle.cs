using UnityEngine;
using System.Collections;

public class PanelToggle : MonoBehaviour {

	public UIButton mapButton;
	public UIButton attacksButton;
	public UIButton unitsButton;
	public UIButton typeButton;

	public UIPanel mapPanel;
	public UIPanel attacksPanel;
	public UIPanel unitsPanel;
	public UIPanel typePanel;

	public Color activeColor;
	public Color inactiveColor;

	public void OpenMap() 
	{
		mapPanel.gameObject.SetActive(true);
		attacksPanel.gameObject.SetActive(false);
		unitsPanel.gameObject.SetActive(false);
		typePanel.gameObject.SetActive(false);

		mapButton.defaultColor = activeColor;
		attacksButton.defaultColor = inactiveColor;
		unitsButton.defaultColor = inactiveColor;
		typeButton.defaultColor = inactiveColor;
	}

	public void OpenAttacks()
	{
		mapPanel.gameObject.SetActive(false);
		attacksPanel.gameObject.SetActive(true);
		unitsPanel.gameObject.SetActive(false);
		typePanel.gameObject.SetActive(false);
		
		mapButton.defaultColor = inactiveColor;
		attacksButton.defaultColor = activeColor;
		unitsButton.defaultColor = inactiveColor;
		typeButton.defaultColor = inactiveColor;
	}

	public void OpenUnits()
	{
		mapPanel.gameObject.SetActive(false);
		attacksPanel.gameObject.SetActive(false);
		unitsPanel.gameObject.SetActive(true);
		typePanel.gameObject.SetActive(false);
		
		mapButton.defaultColor = inactiveColor;
		attacksButton.defaultColor = inactiveColor;
		unitsButton.defaultColor = activeColor;
		typeButton.defaultColor = inactiveColor;
	}

	public void OpenTypes()
	{
		mapPanel.gameObject.SetActive(false);
		attacksPanel.gameObject.SetActive(false);
		unitsPanel.gameObject.SetActive(false);
		typePanel.gameObject.SetActive(true);
		
		mapButton.defaultColor = inactiveColor;
		attacksButton.defaultColor = inactiveColor;
		unitsButton.defaultColor = inactiveColor;
		typeButton.defaultColor = activeColor;
	}
}
