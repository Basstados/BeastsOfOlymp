using UnityEngine;
using System.Collections;

namespace GameDataUI {
	public class GameDataEditControl : MonoBehaviour {

		public MapDataPanel mapPanel;
		public AttacksPanel attacksPanel;
		public UnitPanel unitPanel;
		public TypePanel typePanel;

		// Use this for initialization
		void Awake () 
		{
			Database.LoadFromFile();
			Debug.Log ("Datas loaded from file!");

			mapPanel.Init(Database.GetMapData());
			attacksPanel.Init(Database.GetAttacks());
			unitPanel.Init(Database.GetUnitsData());
			typePanel.Init(Database.GetTypes ());
		}

		public void UpdateDatabase()
		{
			mapPanel.Save();
			attacksPanel.Save();
			unitPanel.Save();
			typePanel.Save();

			attacksPanel.Refresh();
			unitPanel.Refresh();
			typePanel.Refresh();
		}

		public void Save() 
		{
			UpdateDatabase();
			Database.SaveAsFile();
		}
	}
}