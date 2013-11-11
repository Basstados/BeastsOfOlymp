using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class CombatMenu : MonoBehaviour {
	
	UIPanel panel;

	// Use this for initialization
	void Start () {
		panel = GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MoveTo( Vector3 screenPos ) {
		panel.enabled = true;
		screenPos.x -= Screen.width/2;
		screenPos.y -= Screen.height/2;
		transform.localPosition = screenPos;	
	}
	
	public void ActionAttack() {
		Debug.Log("Button Clicked");	
	}
	
	public void ActionMove() {
			
	}
	
	public void Hide() {
		panel.enabled = false;
	}
}
