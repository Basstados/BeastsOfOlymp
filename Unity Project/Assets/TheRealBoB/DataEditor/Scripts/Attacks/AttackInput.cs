using UnityEngine;
using System.Collections;

public class AttackInput : MonoBehaviour {

	public UIInput nameInput;
	public UIInput apInput;
	public UIInput damageInput;
	public UIInput hitInput;
	public UIInput rangeInput;
	
	char[] trimChar = new char[]{'|'};
	AttacksPanel parent;

	#region properties
	public string name {
		get {
			return nameInput.value;
		}
		set {
			nameInput.value = value;
		}
	}

	public int apCost {
		get {
			return int.Parse(apInput.value.TrimEnd(trimChar));
		}
		set {
			apInput.value = value.ToString();
		}
	}

	public int damage {
		get  {
			return int.Parse(damageInput.value.TrimEnd(trimChar));
		}
		set {
			damageInput.value = value.ToString();
		}
	}

	public float hitChance {
		get  {
			return float.Parse(hitInput.value.TrimEnd(trimChar));
		}
		set {
			hitInput.value = value.ToString();
		}
	}

	public int range {
		get  {
			return int.Parse(rangeInput.value.TrimEnd(trimChar));
		}
		set {
			rangeInput.value = value.ToString();
		}
	}
	#endregion

	public void Init(Attack attack, AttacksPanel parent) 
	{
		this.parent = parent;
		if(attack.name != null)
			name = attack.name;
		apCost = attack.apCost;
		damage = attack.damage;
		hitChance = attack.hitChance;
		range = attack.range;
	}

	public Attack GetAttack() 
	{
		Attack atk = new Attack();
		atk.name = name;
		atk.apCost = apCost;
		atk.damage = damage;
		atk.hitChance = hitChance;
		atk.range = range;

		return atk;
	}

	public void Delete() 
	{
		parent.Remove(this);
		Destroy(this.gameObject);
	}
}
