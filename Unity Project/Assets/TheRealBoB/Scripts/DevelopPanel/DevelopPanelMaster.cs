using UnityEngine;
using System.Collections;

public class DevelopPanelMaster : MonoBehaviour {

//	Controller controller;
	Model model;

	public UILabel roundLabel;
	public UILabel activeUnitLabel;


	public void Init(Controller controller, Model model, BView bView) 
	{
//		this.controller = controller;
		this.model = model;
	}
	
	// Update is called once per frame
	void Update () {
		// TODO dosn't work in build because of missing references
//		roundLabel.text = "Round: " + model.combat.round;
//		activeUnitLabel.text = "Active unit: " + model.activeUnit;
	}
}
