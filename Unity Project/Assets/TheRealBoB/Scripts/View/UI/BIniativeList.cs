using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BIniativeList : MonoBehaviour {

	public GameObject unitLabelPrefab;
	public GameObject anchor;

	public float height = 20f;
	public float widthOffset = 100f;

	List<BIniativeListUnit> unitIcons = new List<BIniativeListUnit>();
	int[] positions;
	BIniativeListUnit currentIcon;

	private void AddUnit(Unit unit)
	{
		GameObject handle = Instantiate(unitLabelPrefab) as GameObject;
		handle.transform.parent = anchor.transform;
		handle.transform.localScale = Vector3.one;
		handle.transform.localPosition = Vector3.zero;
		handle.GetComponent<BIniativeListUnit>().Init(unit, height);
		unitIcons.Add(handle.GetComponent<BIniativeListUnit>());
		UpdatePositions();
	}

	public void UpdateList(List<Unit> unitList)
	{
		// remove old icons
		// TODO: using destroy is unperformant; may improve this
//		foreach(BIniativeListUnit icon in unitIcons) {
//			Destroy(icon.gameObject);
//		}
//		unitIcons.Clear();
		if(unitIcons.Count == 0) {
			positions = new int[unitList.Count];
			// add new icons
			for (int i = 0; i < unitList.Count; i++) {
				positions[i] = i;
				AddUnit(unitList[i]);
			}
		} else {
			foreach(BIniativeListUnit item in unitIcons) {
				if(!item.unit.Alive) item.SetDead();
			}
		}
	}

	public void ActivateIcon(Unit unit)
	{
		int index = unitIcons.FindIndex(delegate(BIniativeListUnit obj) {
			return obj.unit == unit;
		});
		// active unit gets the first position
		for (int i = 0; i < unitIcons.Count; i++) {
			positions[(index + i) % unitIcons.Count] = i;
		}
		UpdatePositions();
	}

	void UpdatePositions()
	{
		//float offset = unitIcons.Count * height / 2;
		for (int i = 0; i < unitIcons.Count; i++) {
			unitIcons[i].UpdatePosition(positions[i]);
		}
	}
}
