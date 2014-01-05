using UnityEngine;
using System.Collections;

public class GameDataEditControl : MonoBehaviour {

	public MapDataPanel mapPanel;
	public AttacksPanel attacksPanel;

	// Use this for initialization
	void Start () 
	{
		Database.LoadFromFile();

		mapPanel.Init(Database.GetMapData());
		attacksPanel.Init(Database.GetAttacks());
	}

	public void Save() 
	{
		mapPanel.Save();
		attacksPanel.Save();
		Database.SaveAsFile();
	}
}
