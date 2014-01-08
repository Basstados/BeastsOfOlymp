using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDataInput : MonoBehaviour {

	public UIInput nameInput;
	public UIInput healthInput;
	public UIInput attackInput;
	public UIInput initativeInput;
	public UIInput apInput;
	public GameObject addAttackButton;
	public GameObject attackLocator;
	public GameObject attackInputPrefab;
	public float rowHeight;

	List<UnitAttackInput> attacks;
	UnitPanel parent;
	char[] trimChar = new char[]{'|'};

	#region properties
	public string name {
		get {
			return nameInput.value;
		}
		set {
			nameInput.value = value;
		}
	}

	public int health {
		get  {
			return int.Parse(healthInput.value.TrimEnd(trimChar));
		}
		set {
			healthInput.value = value.ToString();
		}
	}

	public int attack {
		get  {
			return int.Parse(attackInput.value.TrimEnd(trimChar));
		}
		set {
			attackInput.value = value.ToString();
		}
	}

	public int initative {
		get  {
			return int.Parse(initativeInput.value.TrimEnd(trimChar));
		}
		set {
			initativeInput.value = value.ToString();
		}
	}

	public int ap {
		get  {
			return int.Parse(apInput.value.TrimEnd(trimChar));
		}
		set {
			apInput.value = value.ToString();
		}
	}
	#endregion

	public void Init(UnitData unitData, UnitPanel parent) 
	{
		this.parent = parent;
		if(unitData.name != null)
			name = unitData.name;
		health = unitData.baseHealth;
		attack = unitData.baseAttack;
		initative = unitData.baseInitiative;
		ap = unitData.baseActionPoints;

		attacks = new List<UnitAttackInput>();
		if(unitData.attackNames != null)
			foreach(string atk in unitData.attackNames) {
				AddAttack(atk);
			}
	}
	
	void UpdatePositions() 
	{
		for (int i = 0; i < attacks.Count; i++) {
			attacks[i].gameObject.name = "Attack " + i;
			attacks[i].gameObject.transform.localPosition = new Vector3(0,-i * rowHeight,0f);
			attacks[i].gameObject.transform.localScale = Vector3.one;
		}
		addAttackButton.transform.localPosition = new Vector3(0,- attacks.Count * rowHeight,0f);
	}

	public void AddAttack() {
		AddAttack("new attack");
	}

	public void AddAttack(string name) 
	{
		GameObject handle = (GameObject) Instantiate(attackInputPrefab);
		handle.transform.parent = attackLocator.transform;
		UnitAttackInput atkInput = handle.GetComponent<UnitAttackInput>();
		atkInput.Init(name, this);
		attacks.Add(atkInput);

		UpdatePositions();
	}

	public void Remove(UnitAttackInput atkInput)
	{
		attacks.Remove(atkInput);
		UpdatePositions();
	}

	public void Delete() 
	{
		parent.Remove(this);
		Destroy(this.gameObject);
	}

	public UnitData GetUnitData()
	{
		UnitData unitData = new UnitData();
		unitData.name = name;
		unitData.baseHealth = health;
		unitData.baseAttack = attack;
		unitData.baseInitiative = initative;
		unitData.baseActionPoints = ap;
		string[] atkNames = new string[attacks.Count];
		for (int i = 0; i < attacks.Count; i++) {
			atkNames[i] = attacks[i].name;
		}
		unitData.attackNames = atkNames;

		return unitData;
	}
}
