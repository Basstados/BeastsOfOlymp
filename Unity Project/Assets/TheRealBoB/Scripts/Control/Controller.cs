using UnityEngine;

using System;

public class Controller
{
	Model model;

	public Controller ()
	{
		Init();
	}

	void Init() {
		EventProxyManager.Instance.RegisterForEvent(EventName.MapTileTapped, HandleMapTileTapped);

		// TODO try to read this from an json-file
		model = new Model();
		model.Init(10, 10);
		model.SpawnUnit(model.mapTiles[5][5]);
	}
	
	void HandleMapTileTapped(object sender, EventArgs args)
	{
		Debug.Log("Tap Event triggert");
	}
}

