using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {

	public GameObject panel;
	public UIButton attackButton;
	public UIButton moveButton;

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
		attackButton.gameObject.SetActive( bUnit.unit.canAttack );
		moveButton.gameObject.SetActive( bUnit.unit.canMove );
	}
	
	public void ActionAttack() 
	{
		panel.SetActive( false );
		EventProxyManager.FireEvent(EventName.AttackActionSelected,this,null);
	}
	
	public void ActionMove() 
	{
		panel.SetActive( false );
		EventProxyManager.FireEvent(EventName.MoveActionSelected,this,new MoveActionSelectedEvent(bUnit));
	}
	
	public void Hide() 
	{
		panel.SetActive( false );
	}
}

public class MoveActionSelectedEvent : System.EventArgs
{
	public BUnit bUnit;

	public MoveActionSelectedEvent (BUnit bUnit)
	{
		this.bUnit = bUnit;
	}
}