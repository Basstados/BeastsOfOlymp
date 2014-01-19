using UnityEngine;
using System.Collections;

public class BAttackButton : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel damageLabel;
	public UILabel apLabel;
	public UISprite typeSprite;

	BCombatMenu parent;
	Attack attack;

	public void Init(Attack attack, BCombatMenu parent) 
	{
		this.parent = parent;
		this.attack = attack;
		nameLabel.text = attack.name;
		damageLabel.text = attack.damage.ToString();
		apLabel.text = attack.apCost.ToString();
		typeSprite.spriteName = attack.type.name;
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
