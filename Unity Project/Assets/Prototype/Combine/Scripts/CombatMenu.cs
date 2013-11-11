using UnityEngine;
using System.Collections;

public class CombatMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MoveTo( Vector3 screenPos ) {
		screenPos.x -= Screen.width/2;
		screenPos.y -= Screen.height/2;
		transform.localPosition = screenPos;	
	}
}
