using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class CombatMenu : MonoBehaviour {
	
	UIPanel panel;
	
	private Monster target;

	// Use this for initialization
	void Start () {
		panel = GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OpenForMonster( Vector3 screenPos, Monster targetMonster ) {
		panel.enabled = true;
		target = targetMonster;
		screenPos.x -= Screen.width/2;
		screenPos.y -= Screen.height/2;
		transform.localPosition = screenPos;	
	}
	
	public void ActionAttack() {
		Debug.Log("Button Clicked");
	}
	
	public void ActionMove() {
		target.SendMessage("InitMove");
		panel.enabled = false;
	}
	
	public void Hide() {
		panel.enabled = false;
	}
}
