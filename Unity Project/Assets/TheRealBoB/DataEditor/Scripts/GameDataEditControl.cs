using UnityEngine;
using System.Collections;

namespace GameDataUI {
	public class GameDataEditControl : MonoBehaviour {

		public MapDataPanel mapPanel;
		public AttacksPanel attacksPanel;
		public UnitPanel unitPanel;

		// Use this for initialization
		void Awake () 
		{
			Database.LoadFromFile();
			Debug.Log ("Datas loaded from file!");

			mapPanel.Init(Database.GetMapData());
			attacksPanel.Init(Database.GetAttacks());
			unitPanel.Init(Database.GetUnitsData());
		}

		public void Save() 
		{
			mapPanel.Save();
			attacksPanel.Save();
			unitPanel.Save();
			Database.SaveAsFile();
		}
	}
}