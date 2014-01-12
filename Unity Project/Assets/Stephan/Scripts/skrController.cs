using UnityEngine;
using System.Collections;

public class skrController {

	skrModel model;
	skrBMap bMap;

	public event System.EventHandler<InitializedEvent>  Initialized;
	void OnInitialized (skrMapTile[,] mapTiles) {
		if (Initialized != null)
			Initialized (this, new InitializedEvent(mapTiles) );
	}

	public skrController ( skrBMap bMap)
	{
		this.bMap = bMap;
	}

	public void Init(int width, int height) {
		model = new skrModel(width, height);
		model.UnitMoved += bMap.HandleUnitMoved;
		model.UnitSpawned += bMap.HandleUnitSpawned;
		model.UnitSpawned += HandleUnitSpawned;
		model.UnitActivated += HandleUnitActivated;
		OnInitialized(model.MapTiles);

		// just for tests
		model.SpawnUnit(5, 5);
	}

	void HandleUnitActivated (object sender, skrUnitActivatedEvent e)
	{
		Debug.Log ("Unit " + e.unit + " has been activated");
	}

	void HandleUnitSpawned (object sender, skrUnitSpawnedEvent e)
	{
		// just for testing activate the last spawned unit
		model.ActivateUnit(e.unit);
	}

	public void MoveUnit (skrUnit unit, skrMapTile target) 
	{
		model.MoveUnit (unit, target);
	}

	public void HandleMapTileTapped(object sender, skrMapTileTappedEvent e) {
		Debug.Log("MapTile tapped!");
		MoveUnit(model.ActiveUnit, e.mapTile);
	}
}

public class InitializedEvent : System.EventArgs {
	public skrMapTile[,] maptiles;

	public InitializedEvent (skrMapTile[,] maptiles)
	{
		this.maptiles = maptiles;
	}
}
