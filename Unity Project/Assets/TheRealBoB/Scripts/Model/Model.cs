using System;
using System.Collections.Generic;

public class Model
{
	// representation of the map
	public MapTile[][] mapTiles;
	// all units in the game
	List<Unit> units = new List<Unit>();
	// stores the current combat stiuation
	Combat combat;

	public Model() 
	{
	}

	public void Init(int mapWidth, int mapHeight)
	{
		// init 1st dimension 
		mapTiles = new MapTile[mapWidth][];
		
		for (int i = 0; i < mapWidth; i++) {
			// init 2nd dimension
			mapTiles[i] = new MapTile[mapHeight];
			
			for (int j = 0; j < mapHeight; j++) {
				mapTiles[i][j] = new MapTile(i,j);
			}
		}
		// map is now initiliazed
		EventProxyManager.Instance.FireEvent(EventName.Initialized, this, new MapInitializedEvent (mapTiles));
	}

	public void SpawnUnit(MapTile mapTile)
	{
		// IMPORTANT! Keep refence synced
		Unit unit = new Unit();
		unit.mapTile = mapTile;
		mapTile.unit = unit;
		// unit is spawned
		EventProxyManager.Instance.FireEvent(EventName.UnitSpawned, this, new UnitSpawnedEvent (unit));
	}


}

public class MapInitializedEvent : System.EventArgs
{
	public MapTile[][] mapTiles;

	public MapInitializedEvent (MapTile[][] mapTiles)
	{
		this.mapTiles = mapTiles;
	}
}

public class UnitSpawnedEvent : System.EventArgs 
{
	public Unit unit;

	public UnitSpawnedEvent (Unit unit)
	{
		this.unit = unit;
	}
}
