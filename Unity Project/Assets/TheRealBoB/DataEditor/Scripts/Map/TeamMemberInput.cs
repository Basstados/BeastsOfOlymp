using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamMemberInput : MonoBehaviour {

	int teamSize;
	public UIInput teamSizeInput;

	public GameObject tmInputPrefab;
	public float tmInputSize = 25f;

	MapData.TeamUnit[] units;
	TeamMemberInputRow[] unitFields;

	public void Init(MapData.TeamUnit[] units) {
		this.units = units;

		teamSizeInput.value = units.Length.ToString();
		GenerateTeamMemberInputs();

		for (int i = 0; i < units.Length; i++) {
			unitFields[i].name = units[i].name;
			unitFields[i].x = units[i].position.x;
			unitFields[i].y = units[i].position.y;
		}
	}

	public void GenerateTeamMemberInputs() 
	{
		if(tmInputPrefab == null)
			return;

		// clear children
		List<GameObject> children = new List<GameObject>();
		foreach(Transform child in transform) children.Add(child.gameObject);
		foreach(GameObject child in children) Destroy(child);

		char[] trimChar = new char[]{'|'};
		teamSize = int.Parse(teamSizeInput.value.TrimEnd(trimChar));

		unitFields = new TeamMemberInputRow[teamSize];

		for (int i = 0; i < teamSize; i++) {
			GameObject handle = (GameObject) Instantiate(tmInputPrefab);
			handle.name = "Teammember " + i;
			handle.transform.parent = this.transform;
			handle.transform.localPosition = new Vector3(0, - tmInputSize * i, 0);
			handle.transform.localScale = Vector3.one;

			unitFields[i] = handle.GetComponent<TeamMemberInputRow>();
		}
	}

	public MapData.TeamUnit[] GetTeamUnits ()
	{
		MapData.TeamUnit[] team = new MapData.TeamUnit[unitFields.Length];
		for (int i = 0; i < unitFields.Length; i++) {
			team[i] = new MapData.TeamUnit(unitFields[i].name, new Point(unitFields[i].x,unitFields[i].y));
		}

		return team;
	}
}
