using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BIniativeList : MonoBehaviour {

	public GameObject unitLabelPrefab;
	public GameObject anchor;

	public float height = 20f;
	public float widthOffset = 100f;

	List<BIniativeListUnit> unitIcons = new List<BIniativeListUnit>();
	BIniativeListUnit currentIcon;

	private void AddUnit(Unit unit)
	{
		GameObject handle = Instantiate(unitLabelPrefab) as GameObject;
		handle.transform.parent = anchor.transform;
		handle.GetComponent<BIniativeListUnit>().Init(unit);
		unitIcons.Add(handle.GetComponent<BIniativeListUnit>());
		UpdatePositions();
	}

	public void UpdateList(List<Unit> unitList)
	{
		// remove old icons
		// TODO: using destroy is unperformant; may improve this
		foreach(BIniativeListUnit icon in unitIcons) {
			Destroy(icon.gameObject);
		}
		unitIcons.Clear();

		// add new icons
		foreach(Unit unit in unitList) {
			AddUnit(unit);

			EventProxyManager.FireEvent(this, new DebugLogEvent("InitList Unit: " + unit.Name + " " + unit.team));
		}
	}

	public void ActivateIcon(Unit unit)
	{
		foreach(BIniativeListUnit icon in unitIcons) {
			if(icon.unit == unit) {
				icon.Reset();
				icon.SetActive();
			} else {
				icon.Reset();
			}
		}
	}

	void UpdatePositions()
	{
		float offset = unitIcons.Count * height / 2;
		for (int i = 0; i < unitIcons.Count; i++) {
			unitIcons[i].transform.localPosition = new Vector3(widthOffset, offset - height * i, 0);
		}
	}
}
