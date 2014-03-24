using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameDataUI {
	public class TeamMemberInput : MonoBehaviour, IInputListParent {

		public GameObject addTeamMemberButton;

		public GameObject tmInputPrefab;
		public float tmInputSize = 25f;

		MapData.TeamUnit[] units;
		List<TeamMemberInputRow> unitFields = new List<TeamMemberInputRow>();

		public void Init(MapData.TeamUnit[] units) {
			this.units = units;

			for (int i = 0; i < units.Length; i++) {
				AddTeamMember(units[i]);
			}
			UpdatePositions();
		}

		public void AddTeamMember() 
		{
			AddTeamMember(new MapData.TeamUnit());
		}

		public void AddTeamMember(MapData.TeamUnit unit) 
		{
			string[] options = GetUnitOptions();
			GameObject handle = (GameObject) Instantiate(tmInputPrefab);
			handle.name = "Teammember";
			handle.transform.parent = this.transform;

			TeamMemberInputRow tmInput = handle.GetComponent<TeamMemberInputRow>();
			unitFields.Add(tmInput);
			tmInput.Init(unit.name, unit.position.x, unit.position.y, options, this);

			UpdatePositions();
		}

		public void UpdatePositions()
		{
			for (int i = 0; i < unitFields.Count; i++) {
				unitFields[i].gameObject.transform.localPosition = new Vector3(0, - tmInputSize * i, 0);
				unitFields[i].gameObject.transform.localScale = Vector3.one;
			}
			addTeamMemberButton.transform.localPosition = new Vector3(0,-tmInputSize * unitFields.Count,0);
		}

		public MapData.TeamUnit[] GetTeamUnits ()
		{
			MapData.TeamUnit[] team = new MapData.TeamUnit[unitFields.Count];
			for (int i = 0; i < unitFields.Count; i++) {
				team[i] = new MapData.TeamUnit(unitFields[i].rowName, new Vector(unitFields[i].x,unitFields[i].y));
			}

			return team;
		}

		public void Refresh()
		{
			string[] options = GetUnitOptions();
			foreach(TeamMemberInputRow tmInput in unitFields) {
				tmInput.UpdateOptions(options);
			}
		}

		string[] GetUnitOptions()
		{
			string[] options = new string[GameData.GetUnitsData().Length];
			for (int i = 0; i < options.Length; i++) {
				options[i] = GameData.GetUnitsData()[i].name;
			}
			return options;
		}

		public void RemoveListElement (IInputListElement child)
		{
			unitFields.Remove((TeamMemberInputRow) child);
			UpdatePositions();
		}
	}
}
