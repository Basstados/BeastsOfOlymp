using UnityEngine;
using System.Collections;

public class DevelopPanelMaster : MonoBehaviour {

	Controller controller;
	Model model;
	BView bView;

	public UILabel roundLabel;
	public UILabel activeUnitLabel;


	public void Init(Controller controller, Model model, BView bView) 
	{
		this.controller = controller;
		this.model = model;
		this.bView = bView;
	}
	
	// Update is called once per frame
	void Update () {
		roundLabel.text = "Round: " + model.combat.round;
		activeUnitLabel.text = "Active unit: " + model.activeUnit;
	}
}
