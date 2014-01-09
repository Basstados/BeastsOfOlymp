using UnityEngine;
using System.Collections;

public class BUnitUI : MonoBehaviour {

	public UISlider lifebar;
	public Color playerColor;
	public Color aiColor;
	public UILabel nameLabel;
	public BapBar apbar;
	BUnit parent;

	bool initalized = false;

	UISprite[] apMarker;

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

		apbar.Init(parent.unit.MaxActionPoints);

		initalized = true;
	}

	public void UpdateLivebar() 
	{
		if(!initalized) return;

		lifebar.value = parent.unit.HealthPoints / (float) parent.unit.MaxHealthPoints;
	}

	public void UpdateAPBar()
	{
		apbar.EmptyAP(parent.unit.ActionPoints);
	}
}
