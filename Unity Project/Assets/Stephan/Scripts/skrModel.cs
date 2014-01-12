using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class skrModel {

	skrMapTile[,] mapTiles;
	skrUnit activeUnit;
	List<skrUnit> units;

	public event System.EventHandler<skrUnitMovedEvent> UnitMoved;
	void OnUnitMoved(skrMapTile source, skrMapTile target, skrUnit unit) {
		if (UnitMoved != null)
			UnitMoved (this, new skrUnitMovedEvent(source, target, unit));
	}

	public event System.EventHandler<skrUnitActivatedEvent> UnitActivated;
	void OnUnitActivated(skrUnit unit) {
		if (UnitActivated != null)
			UnitActivated (this, new skrUnitActivatedEvent (unit));
	}

	public event System.EventHandler<skrUnitSpawnedEvent> UnitSpawned;
	void OnUnitSpawned(skrUnit unit) 
	{
		if(UnitSpawned != null)
			UnitSpawned(this, new skrUnitSpawnedEvent(unit));
	}

	public skrModel(int width, int height) {
		mapTiles = new skrMapTile[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				mapTiles[i,j] = new skrMapTile();
			}
		}
	}
	
	public skrMapTile[,] MapTiles {
		get {
			return mapTiles;
		}
	}

	public skrUnit ActiveUnit {
		get {
			return activeUnit;
		}
	}

	public void SpawnUnit(int x, int y) {
		if(units == null)
			units = new List<skrUnit>();

		// instatiate unit and link it to the given mapTile
		skrUnit unit = new skrUnit();
		unit.mapTile = mapTiles[x,y];
		mapTiles[x,y].unit = unit;
		units.Add(unit);

		OnUnitSpawned(unit);
	}

	public void MoveUnit (skrUnit unit, skrMapTile target) 
	{
		skrMapTile source = unit.mapTile;
		target.unit = unit;
		source.unit = null;
		unit.mapTile = target;
		OnUnitMoved (source, target, unit);
	}

	public void ActivateUnit(skrUnit unit) {
		activeUnit = unit;
		OnUnitActivated(unit);
	}
}


public class skrUnitMovedEvent : System.EventArgs 
{
	public skrMapTile source;
	public skrMapTile target;
	public skrUnit unit;

	public skrUnitMovedEvent (skrMapTile source, skrMapTile target, skrUnit unit)
	{
		this.source = source;
		this.target = target;
		this.unit = unit;
	}
}

public class skrUnitActivatedEvent : System.EventArgs
{
	public skrUnit unit;

	public skrUnitActivatedEvent (skrUnit unit)
	{
		this.unit = unit;
	}
}

public class skrUnitSpawnedEvent : System.EventArgs
{
	public skrUnit unit;

	public skrUnitSpawnedEvent (skrUnit unit)
	{
		this.unit = unit;
	}
	
}