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
			GameData.LoadFromFile();
			Debug.Log ("Datas loaded from file!");

			mapPanel.Init(GameData.GetMapData());
			attacksPanel.Init(GameData.GetAttacks());
			unitPanel.Init(GameData.GetUnitsData());
			typePanel.Init(GameData.GetTypes ());
		}

		public void UpdateDatabase()
		{
			mapPanel.Save();
			attacksPanel.Save();
			unitPanel.Save();
			typePanel.Save();

			mapPanel.Refresh();
			attacksPanel.Refresh();
			unitPanel.Refresh();
			typePanel.Refresh();
		}

		public void Save() 
		{
			UpdateDatabase();
			GameData.SaveAsFile();
		}
	}
}