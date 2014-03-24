using UnityEngine;
using System.Collections.Generic;

public class AttackInput : MonoBehaviour {

	public UIInput nameInput;
	public UIInput apInput;
	public UIInput damageInput;
	public UIInput hitInput;
	public UIInput rangeInput;
	public UIPopupList typeInput;
	public AreaInput areaInput;

	char[] trimChar = new char[]{'|'};
	AttacksPanel parent;

	#region properties
	public string atkName {
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
		if(attack.attackName != null)
			atkName = attack.attackName;
		damage = attack.damage;
		hitChance = attack.hitChance;
		range = attack.range;
		typeInput.value = attack.type.name;
		areaInput.Refresh(attack.area);

		if(attack.type.name != null) typeInput.value = attack.type.name;
		Refresh();
	}

	public Attack GetAttack() 
	{
		Attack atk = new Attack();
		atk.attackName = atkName;
		atk.damage = damage;
		atk.hitChance = hitChance;
		atk.range = range;
		atk.type = GameData.GetType(typeInput.value);
		atk.area = areaInput.GetArea();
		return atk;
	}

	public void Refresh() 
	{
		string[] options = GetTypeOptions();
		if(options.Length > 0) typeInput.items = new List<string>(options);
	}

	string[] GetTypeOptions()
	{
		string[] options = new string[GameData.GetTypes().Length];
		for(int i = 0; i < options.Length; i++) {
			options[i] = GameData.GetTypes()[i].name;
		}
		return options;
	}

	public void Delete() 
	{
		parent.Remove(this);
		Destroy(this.gameObject);
	}
}
