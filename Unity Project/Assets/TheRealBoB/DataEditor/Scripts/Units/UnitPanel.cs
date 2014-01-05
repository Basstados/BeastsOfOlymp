using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitPanel : MonoBehaviour {

	public GameObject unitInputPrefab;
	public GameObject addUnitButton;
	public float columnWidth = 420f;
	public float rowHeight = 170f;
	
	int columns = 3;
	
	List<UnitDataInput> unitInputs;
	
	public void Init(UnitData[] unitDatas) 
	{
		unitInputs = new List<UnitDataInput>();
		
		for (int i = 0; i < unitDatas.Length; i++) {
			Add(unitDatas[i]);
		}
		
		UpdatePositions();
	}
	
	void UpdatePositions() 
	{
		for (int i = 0; i < unitInputs.Count; i++) {
			unitInputs[i].gameObject.name = "Unit " + i;
			unitInputs[i].gameObject.transform.localPosition = new Vector3((i%columns) * columnWidth,-(i/columns) * rowHeight,0f);
			unitInputs[i].gameObject.transform.localScale = Vector3.one;
		}
		addUnitButton.transform.localPosition = new Vector3((unitInputs.Count%columns) * columnWidth,-(unitInputs.Count/columns) * rowHeight,0f);
	}
	
	public void Add() 
	{
		Add (new UnitData());
	}

	public void Add(UnitData unitData) 
	{
		GameObject handle = (GameObject) Instantiate(unitInputPrefab);
		handle.transform.parent = this.transform;
		UnitDataInput unitInput = handle.GetComponent<UnitDataInput>();
		unitInput.Init(unitData, this);
		unitInputs.Add(unitInput);
		
		UpdatePositions();
	}
	
	public void Remove(UnitDataInput unitInput) 
	{
		unitInputs.Remove(unitInput);
		UpdatePositions();
	}
	
	public void Save()
	{
		UnitData[] units = new UnitData[unitInputs.Count];
		for (int i = 0; i < unitInputs.Count; i++) {
			units[i] = unitInputs[i].GetUnitData();
		}

		Database.ClearUnits();
		Database.AddUnitDataArray(units);
		Debug.Log("All the UnitDatas where saved! Hurray!");
	}
}
