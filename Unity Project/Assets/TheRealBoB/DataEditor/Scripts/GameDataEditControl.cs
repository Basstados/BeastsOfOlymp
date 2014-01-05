using UnityEngine;
using System.Collections;

public class GameDataEditControl : MonoBehaviour {

	public MapDataPanel mapPanel;
	public AttacksPanel attacksPanel;
	public UnitPanel unitPanel;

	// Use this for initialization
	void Start () 
	{
		Database.LoadFromFile();

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
