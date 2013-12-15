using System;
using System.Collections.Generic;
using Algorithms;

public class Model
{
	// representation of the map
	public MapTile[][] mapTiles;
	// all units in the game
	public List<Unit> units = new List<Unit>();
    public Unit activeUnit = new Unit();
	// stores the current combat stiuation
	public Combat combat = new Combat();

	public bool matchRunning = true;

	public Model() 
	{
	}

	public void InitMap(MapData mapData)
	{
		// init 1st dimension 
		mapTiles = new MapTile[mapData.width][];

		// instantiate maptile from mapData
		for (int i = 0; i < mapData.width; i++) {
			// init 2nd dimension
			mapTiles[i] = new MapTile[mapData.height];
			
			for (int j = 0; j < mapData.height; j++) {
				mapTiles[i][j] = new MapTile(i,j);
				mapTiles[i][j].Penalty = mapData.penaltys[i][j];
			}
		}

		// map is now initiliazed
		EventProxyManager.FireEvent(this, new MapInitializedEvent (mapTiles));
	}

	public void InitUnits(MapData mapData)
	{
		// spawn units for all teams
		int teamID = 0;
		foreach(MapData.TeamUnit[] team in mapData.teamUnits) {
			foreach(MapData.TeamUnit tUnit in team) {
				SpawnUnit(mapTiles[tUnit.position.x][tUnit.position.y], (Unit.Team)teamID, tUnit.name);
			}
			teamID++;
		}
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
	public byte[,] GetMoveGrid() 
	{
		byte[,] grid = new byte[mapTiles.Length,mapTiles[0].Length];

		for (int i = 0; i < mapTiles.Length; i++) {
			for (int j = 0; j < mapTiles[i].Length; j++) {
				grid[i,j] = mapTiles[i][j].Penalty;
			}
		}

		return grid;
	}

	/// <summary>
	/// Get the penalty values of each MapTile combines as matrix.
	/// </summary>
	/// <returns>The grid (penalty matrix)</returns>
	public byte[,] GetAttackGrid() 
	{
		byte[,] grid = new byte[mapTiles.Length,mapTiles[0].Length];
		
		for (int i = 0; i < mapTiles.Length; i++) {
			for (int j = 0; j < mapTiles[i].Length; j++) {
				grid[i,j] = mapTiles[i][j].PenaltyIgnoreUnit;
			}
		}
		
		return grid;
	}

	public List<Unit> GetUnitsFromTeam(Unit.Team team) 
	{
		List<Unit> teamList = new List<Unit>();
		foreach(Unit unit in units) {
			if(unit.team == team)
				teamList.Add(unit);
		}
		return teamList;
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
			cost += mapTile.Penalty;
		}
		// don't include the first MapTile; Unit already sits on this MapTile
		cost -= path[0].Penalty;

		return cost;
	}

	public void SpawnUnit(MapTile mapTile, Unit.Team team, string name)
	{
		// IMPORTANT! Keep refence synced
		Unit unit = new Unit();
		unit.mapTile = mapTile;
		mapTile.unit = unit;

		unit.Init(Database.GetUnitData(name));
		unit.team = team;
		if(Unit.Team.AI == team)
			unit.ai = new ArtificalIntelligence(this, unit);

		// add unit to list
		units.Add(unit);

		// unit is spawned
		EventProxyManager.FireEvent(this, new UnitSpawnedEvent (unit));
	}

    public void ActivateUnit(Unit unit)
    {
        activeUnit = unit;
		EventProxyManager.FireEvent(this, new UnitActivatedEvent(unit));
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

public class MapInitializedEvent : EventProxyArgs
{
	public MapTile[][] mapTiles;

	public MapInitializedEvent (MapTile[][] mapTiles)
	{
		this.name = EventName.Initialized;
		this.mapTiles = mapTiles;
	}
}

public class UnitSpawnedEvent : EventProxyArgs
{
	public Unit unit;

	public UnitSpawnedEvent (Unit unit)
	{
		this.name = EventName.UnitSpawned;
		this.unit = unit;
	}
}

public class UnitActivatedEvent : EventProxyArgs
{
	public Unit unit;

	public UnitActivatedEvent (Unit unit)
	{
		this.name = EventName.UnitActivated;
		this.unit = unit;
	}
}
