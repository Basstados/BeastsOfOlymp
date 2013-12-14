using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BIniativeList : MonoBehaviour {

	public GameObject unitLabelPrefab;
	public GameObject anchor;

	public float width = 120f;
	public float heightOffset = - 30f;

	List<BIniativeListUnit> unitIcons = new List<BIniativeListUnit>();
	BIniativeListUnit currentIcon;

	public void AddUnit(Unit unit)
	{
		GameObject handle = Instantiate(unitLabelPrefab) as GameObject;
		handle.transform.parent = anchor.transform;
		handle.GetComponent<BIniativeListUnit>().Init(unit);
		unitIcons.Add(handle.GetComponent<BIniativeListUnit>());
		UpdatePositions();
	}

	public void ActivateIcon(Unit unit)
	{
		foreach(BIniativeListUnit icon in unitIcons) {
			if(icon.unit == unit) {
				icon.SetActive();
			} else {
				icon.Reset();
			}
		}
	}

	void UpdatePositions()
	{
		unitIcons.Sort();
		float offset = -1 * unitIcons.Count * width / 2;
		for (int i = 0; i < unitIcons.Count; i++) {
			unitIcons[i].transform.localPosition = new Vector3(offset + width * i, heightOffset, 0);
		}
	}
}
