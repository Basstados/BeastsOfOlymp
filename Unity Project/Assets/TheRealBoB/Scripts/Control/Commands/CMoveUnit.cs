using System;
using System.Collections.Generic;
using Algorithms;

public class CMoveUnit : ICommand
{
	Model model;
	Controller controller;
	Unit unit;
	MapTile target;

	public CMoveUnit (Model model, Unit unit, MapTile target, Controller controller)
	{
		this.model = model;
		this.unit = unit;
		this.target = target;
		this.controller = controller;
	}


	public void Execute()
	{
		Path path = new Path(new MapTile[]{unit.mapTile}, 0);
		if(unit.mapTile != target) {
			// get path from pathfinder
			path = controller.GetPath(unit.mapTile, target);
			// stop if target is to for unit move
			if(path.Cost > unit.ActionPoints)
				return;

			// we are now sure, that unit is allowed to move and target is in range
			// now performce actual move
			model.MoveUnit(unit, target);
			unit.UseAP(path.Cost);
		}
		unit.CanMove = false;
		
		// after movement fire event
		EventProxyManager.FireEvent(this,new UnitMovedEvent(unit, path));
	}
}

public class UnitMovedEvent : EventProxyArgs
{
	public Unit unit;
	public Path path;

	public UnitMovedEvent (Unit unit, Path path)
	{
		this.name = EventName.UnitMoved;
		this.unit = unit;
		this.path = path;
	}
}
