using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {

	public GameObject panel;
	public UIButton attackButton;
	public UIButton moveButton;

	public void OpenForBUnit(BUnit bUnit) 
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(bUnit.transform.position);
		panel.SetActive(true);
		screenPosition.x -= Screen.width/2;
		screenPosition.y -= Screen.height/2;
		transform.localPosition = screenPosition;
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
		EventProxyManager.FireEvent(EventName.MoveActionSelected,this,null);
	}
	
	public void Hide() 
	{
		panel.SetActive( false );
	}
}
