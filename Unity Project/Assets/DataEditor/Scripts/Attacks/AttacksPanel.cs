using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttacksPanel : MonoBehaviour {

	public GameObject attackInputPrefab;
	public GameObject addAttackButton;
	public float columnWidth = 420f;
	public float rowHeight = 110f;

	int columns = 3;

	List<AttackInput> attackInputs;

	public void Init(Attack[] attacks) 
	{
		attackInputs = new List<AttackInput>();

		for (int i = 0; i < attacks.Length; i++) {
			GameObject handle = (GameObject) Instantiate(attackInputPrefab);
			handle.transform.parent = this.transform;
			AttackInput atkInput = handle.GetComponent<AttackInput>();
			atkInput.Init(attacks[i], this);
			attackInputs.Add(atkInput);
		}

		UpdatePositions();
	}

	void UpdatePositions() 
	{
		for (int i = 0; i < attackInputs.Count; i++) {
			attackInputs[i].gameObject.name = "Attack " + i;
			attackInputs[i].gameObject.transform.localPosition = new Vector3((i%columns) * columnWidth,-(i/columns) * rowHeight,0f);
			attackInputs[i].gameObject.transform.localScale = Vector3.one;
		}
		addAttackButton.transform.localPosition = new Vector3((attackInputs.Count%columns) * columnWidth,-(attackInputs.Count/columns) * rowHeight,0f);
	}

	public void Add() 
	{
		GameObject handle = (GameObject) Instantiate(attackInputPrefab);
		handle.transform.parent = this.transform;
		AttackInput atkInput = handle.GetComponent<AttackInput>();
		atkInput.Init(new Attack(), this);
		attackInputs.Add(atkInput);

		UpdatePositions();
	}

	public void Remove(AttackInput atkInput) 
	{
		attackInputs.Remove(atkInput);
		UpdatePositions();
	}

	public void Refresh()
	{
		foreach(AttackInput input in attackInputs) {
			input.Refresh();
		}
	}

	public void Save()
	{
		Attack[] atks = new Attack[attackInputs.Count];
		for (int i = 0; i < attackInputs.Count; i++) {
			atks[i] = attackInputs[i].GetAttack();
		}

		GameData.ClearAttacks();
		GameData.AddAttackArray(atks);
		Debug.Log("AttackDatas saved!");
	}
}
