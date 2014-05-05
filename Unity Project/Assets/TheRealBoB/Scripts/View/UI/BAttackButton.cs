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
		nameLabel.text = attack.attackName;
		damageLabel.text = attack.damage.ToString();
		typeSprite.spriteName = attack.element.elementName;
		typeBackground.normalSprite = attack.element.elementName + backgroundPostfix;
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
