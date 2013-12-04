using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {

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
		panel.SetActive(true);
		attackButton.gameObject.SetActive(bUnit.unit.canAttack);
		moveButton.gameObject.SetActive(bUnit.unit.canMove);
		backButton.gameObject.SetActive(false);
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
		if(bUnit.unit.canMove || bUnit.unit.canAttack)
			OpenForBUnit (bUnit);
		else
			backButton.gameObject.SetActive(false);
	}
	
	public void Hide() 
	{
		panel.SetActive(false);
	}
}