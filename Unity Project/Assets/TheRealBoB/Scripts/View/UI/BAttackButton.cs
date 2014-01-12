using UnityEngine;
using System.Collections;

public class BAttackButton : MonoBehaviour {

	public UILabel label;

	BCombatMenu parent;
	Attack attack;

	public void Init(Attack attack, BCombatMenu parent) 
	{
		this.parent = parent;
		this.attack = attack;
		label.text = attack.name;
	}

	public void OnClick() 
	{
		parent.ActionAttack(attack);
	}
}
