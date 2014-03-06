using UnityEngine;
using System.Collections;

public class BAttackButton : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel damageLabel;
	public UISprite typeSprite;
	public UIButton typeBackground;
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
		typeBackground.normalSprite = attack.type.name + backgroundPostfix;
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
