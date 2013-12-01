using System;
using System.Collections.Generic;
using Algorithms;

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

	/// <summary>
	/// Get the penalty values of each MapTile combines as matrix.
	/// </summary>
	/// <returns>The grid (penalty matrix)</returns>
	public byte[,] GetGrid() 
	{
		byte[,] grid = new byte[mapTiles.Length,mapTiles[0].Length];

		for (int i = 0; i < mapTiles.Length; i++) {
			for (int j = 0; j < mapTiles[i].Length; j++) {
				grid[i,j] = mapTiles[i][j].penalty;
			}
		}

		return grid;
	}

	public MapTile[] ConvertPathToMapTiles (List<PathFinderNode> path)
	{
		MapTile[] mapTilePath = new MapTile[path.Count];
		
		for (int i = 0; i < path.Count; i++) {
			mapTilePath[i] = mapTiles[path[i].X][path[i].Y];	
		}

		return mapTilePath;
	}

	public int GetPathCost (MapTile[] path)
	{
		int cost = 0;
		foreach(MapTile mapTile in path) {
			cost += mapTile.penalty;
		}
		// don't include the first MapTile; Unit already sits on this MapTile
		cost -= path[0].penalty;

		return cost;
	}

	public void SpawnUnit(MapTile mapTile, Unit.Team team)
	{
		// IMPORTANT! Keep refence synced
		Unit unit = new Unit();
		unit.mapTile = mapTile;
		mapTile.unit = unit;

		// TODO remove since its only a test
		unit.baseStats.movementRange = 2;

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

	public void MoveUnit(Unit unit, MapTile target)
	{
		MapTile start = unit.mapTile;
		// set unit on target MapTile
		unit.mapTile = target;
		target.unit = unit;
		// remove unit from start MapTile
		start.unit = null;
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
