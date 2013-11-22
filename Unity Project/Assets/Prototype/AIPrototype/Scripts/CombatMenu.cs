using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class CombatMenu : MonoBehaviour {
	
	public GameObject panel;
	public UIButton attackButton;
	public UIButton moveButton;
	
	private Monster target;

	// Use this for initialization
	void Start () {
		//panel = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OpenForMonster( Vector3 screenPos, Monster targetMonster ) {
		panel.SetActive( true );
		target = targetMonster;
		screenPos.x -= Screen.width/2;
		screenPos.y -= Screen.height/2;
		transform.localPosition = screenPos;
		attackButton.gameObject.SetActive( !target.hasAttacked );
		moveButton.gameObject.SetActive( !target.hasMoved );
	}
	
	public void ActionAttack() {
		target.SendMessage("InitAttack");
		panel.SetActive( false );
	}
	
	public void ActionMove() {
		target.SendMessage("InitMove");
		panel.SetActive( false );
	}
	
	public void Hide() {
		panel.SetActive( false );
	}
}
