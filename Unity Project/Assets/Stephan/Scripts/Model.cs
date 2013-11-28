using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model {

	MapTile[,] mapTiles;
	skrUnit activeUnit;
	List<skrBUnit> units;

	public event System.EventHandler<UnitMovedEvent> UnitMoved;
	void OnUnitMoved(MapTile source, MapTile target, skrUnit unit) {
		if (UnitMoved != null)
			UnitMoved (this, new UnitMovedEvent(source, target, unit));
	}

	public event System.EventHandler<UnitActivatedEvent> UnitActivated;
	void OnUnitActivated(skrUnit unit) {
		if (UnitActivated != null)
			UnitActivated (this, new UnitActivatedEvent (unit));
	}

	public Model (int width, int height) {
		mapTiles = new MapTile[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				mapTiles[i,j] = new MapTile();
			}
		}
	}
	
	public MapTile[,] MapTiles {
		get {
			return mapTiles;
		}
	}

	public skrUnit ActiveUnit {
		get {
			return activeUnit;
		}
	}

	public void MoveUnit (skrUnit unit, MapTile target) 
	{
		MapTile source = unit.mapTile;
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


public class UnitMovedEvent : System.EventArgs 
{
	public MapTile source;
	public MapTile target;
	public skrUnit unit;

	public UnitMovedEvent (MapTile source, MapTile target, skrUnit unit)
	{
		this.source = source;
		this.target = target;
		this.unit = unit;
	}
}

public class UnitActivatedEvent : System.EventArgs
{
	public skrUnit unit;

	public UnitActivatedEvent (skrUnit unit)
	{
		this.unit = unit;
	}
}