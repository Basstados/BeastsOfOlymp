using UnityEngine;
using System.Collections;

public class Controller {

	Model model;
	skrBMap bMap;

	public event System.EventHandler<InitializedEvent>  Initialized;
	void OnInitialized (MapTile[,] mapTiles) {
		if (Initialized != null)
			Initialized (this, new InitializedEvent(mapTiles) );
	}

	public Controller ( skrBMap bMap)
	{
		this.bMap = bMap;
	}

	public void Init(int width, int height) {
		model = new Model(width, height);
		model.UnitMoved += bMap.HandleUnitMoved;
		OnInitialized(model.MapTiles);

		// just for testing the mapTileTapped Event
	}

	public void MoveUnit (skrUnit unit, MapTile target) 
	{
		model.MoveUnit (unit, target);
	}

	public void HandleMapTileTapped(object sender, MapTileTappedEvent e) {
		Debug.Log("MapTile tapped!");
		// MoveUnit(model.ActiveUnit, e.mapTile);
	}
}

public class InitializedEvent : System.EventArgs {
	public MapTile[,] maptiles;

	public InitializedEvent (MapTile[,] maptiles)
	{
		this.maptiles = maptiles;
	}
}
