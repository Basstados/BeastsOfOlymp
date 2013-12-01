using System;
using System.Collections.Generic;

public class Model
{
	// representation of the map
	public MapTile[][] mapTiles;
	// all units in the game
	public List<Unit> units = new List<Unit>();
    public Unit activeUnit;
	// stores the current combat stiuation
	public Combat combat;

	public Model() 
	{
	}

	public void InitMap(int mapWidth, int mapHeight)
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
		EventProxyManager.FireEvent(EventName.Initialized, this, new MapInitializedEvent (mapTiles));
	}

    public void InitCombat()
    {
        combat = new Combat();
        combat.SetupRound(units);
    }

	public void SpawnUnit(MapTile mapTile, Unit.Team team)
	{
		// IMPORTANT! Keep refence synced
		Unit unit = new Unit();
		unit.mapTile = mapTile;
		mapTile.unit = unit;

		// add unit to list
		units.Add(unit);

		// unit is spawned
		EventProxyManager.FireEvent(EventName.UnitSpawned, this, new UnitSpawnedEvent (unit));
	}

    public void ActivateUnit(Unit unit)
    {
        activeUnit = unit;
		EventProxyManager.FireEvent(EventName.UnitActivated, this, new UnitActivatedEvent(unit));
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

public class UnitActivatedEvent : System.EventArgs
{
	public Unit unit;

	public UnitActivatedEvent (Unit unit)
	{
		this.unit = unit;
	}
}
