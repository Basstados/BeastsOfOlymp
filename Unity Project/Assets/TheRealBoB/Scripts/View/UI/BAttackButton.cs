using UnityEngine;
using System.Collections;

public class BAttackButton : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel damageLabel;
	public UISprite typeSprite;
	public UIButton typeBackground;
	public string backgroundPostfix;
	public UISprite arrowIcon;
	public UISprite swordIcon;


	BCombatMenu parent;
	Attack attack;

	public void Init(Attack attack, BCombatMenu parent) 
	{

		this.parent = parent;
		this.attack = attack;
		nameLabel.text = attack.attackName;
		damageLabel.text = attack.damage.ToString();
		//typeSprite.spriteName = attack.element.elementName;
		typeSprite.spriteName = attack.name;
		typeBackground.normalSprite = attack.element.elementName + backgroundPostfix;

		if (attack.IsRanged ()) 
		{
			//select arrow symbol
			arrowIcon.gameObject.SetActive(true);
			swordIcon.gameObject.SetActive(false);
		} 
		else 
		{
			//select sword symbol
			arrowIcon.gameObject.SetActive(false);
			swordIcon.gameObject.SetActive(true);
		}
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
