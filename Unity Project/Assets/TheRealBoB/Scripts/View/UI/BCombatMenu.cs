using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {

	public GameObject gameoverPanel;
	public UILabel gameoverLabel;
	public GameObject panel;
	public UIButton attackButton;
	public UIButton moveButton;
	public UIButton backButton;

	BUnit bUnit;

	void Update() 
	{
		if(panel.activeSelf) {
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(bUnit.transform.position);
			screenPosition.x -= Screen.width/2;
			screenPosition.y -= Screen.height/2;
			transform.localPosition = screenPosition;
		}
	}

	public void OpenForBUnit(BUnit bUnit) 
	{
		this.bUnit = bUnit;
		if(bUnit.unit.AIControled) {
			panel.SetActive(false);
			backButton.gameObject.SetActive(false);
		} else {
			panel.SetActive(true);
			attackButton.gameObject.SetActive(bUnit.unit.CanAttack);
			moveButton.gameObject.SetActive(bUnit.unit.CanMove);
			backButton.gameObject.SetActive(false);
		}
	}
	
	public void ActionAttack() 
	{
		panel.SetActive(false);
		backButton.gameObject.SetActive(true);
		bUnit.DisplayAttackRange();
	}
	
	public void ActionMove() 
	{
		panel.SetActive(false);
		backButton.gameObject.SetActive(true);
		bUnit.DisplayMovementRange();
	}

	public void ActionEndTurn()
	{
		Hide();
		bUnit.EndTurn();
	}

	public void Back()
	{
		panel.SetActive(true);
		backButton.gameObject.SetActive(false);
		bUnit.ClearDisplayRange();
	}

	public void ActionCompleted()
	{
//		if(bUnit.unit.CanMove || bUnit.unit.CanAttack)
			OpenForBUnit (bUnit);
//		else
//			backButton.gameObject.SetActive(false);
	}
	
	public void Hide() 
	{
		panel.SetActive(false);
	}

	public void DisplayGameover (string text)
	{
		gameoverPanel.SetActive(true);
		gameoverLabel.text = text;
	}
}