using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Beast : MonoBehaviour {

	// base status values
	// these values never change during the game!
	string name = "";
	int baseHealthPoints = 0;
	int baseAttackPoints = 0;
	int baseSpeed = 0;
	int baseSteps = 0;

	// beast icon
	Texture2D icon;

	// current position in a better usable format then Vector3
	Point currentPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
