using UnityEngine;
using System.Collections;

public class BAttackButton : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel damageLabel;
	public UISprite typeSprite;
	public UISprite typeBackgroundSprite;
	public string backgroundPostfix;

	BCombatMenu parent;
	Attack attack;

	public void Init(Attack attack, BCombatMenu parent) 
	{
		this.parent = parent;
		this.attack = attack;
		nameLabel.text = attack.name;
		damageLabel.text = attack.damage.ToString();
		typeSprite.spriteName = attack.type.name;
		typeBackgroundSprite.spriteName = attack.type.name + backgroundPostfix;
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
