using UnityEngine;
using System.Collections;

public class BUnitUI : MonoBehaviour {

	public UISprite typeSprite;
	public UISlider lifebar;
	public Color playerColor;
	public Color aiColor;
	public UILabel nameLabel;
	public BapBar apbar;
	public BDamageNumber damageNumber;
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
		typeSprite.spriteName = parent.unit.data.type.name;

		apbar.Init(parent.unit.MaxActionPoints);

		initalized = true;
	}

	public void MarkAP(int count)
	{
		apbar.ActivateAP(count);
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

	public void ShowDamage(int damage)
	{
		damageNumber.Display(damage.ToString());
	}
}
