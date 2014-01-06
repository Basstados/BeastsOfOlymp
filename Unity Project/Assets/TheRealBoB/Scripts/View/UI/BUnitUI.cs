using UnityEngine;
using System.Collections;

public class BUnitUI : MonoBehaviour {
	
	public UISlider lifebar;
	public Color playerColor;
	public Color aiColor;
	public UILabel nameLabel;
	BUnit parent;

	bool initalized = false;

	public void Init(BUnit parent) 
	{
		this.parent = parent;

		// change color of life bar depending on team
		if(parent.unit.team == Unit.Team.PLAYER) {
			lifebar.foreground.GetComponent<UISprite>().color = playerColor;
		} else {
			lifebar.foreground.GetComponent<UISprite>().color = aiColor;
		}

		// set name label
		nameLabel.text = parent.unit.Name;

		initalized = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if(!initalized) return;

		lifebar.value = parent.unit.HealthPoints / (float) parent.unit.MaxHealthPoints;
	}
}
