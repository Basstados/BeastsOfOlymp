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
		Path path = new Path(new MapTile[]{unit.mapTile});
		// check if start and destination are the same
		if(unit.mapTile != target) {
			// get path from pathfinder
			path = controller.GetPath(unit.mapTile, target);
			// stop if target is to far away for unit move
			if(path.Cost > unit.MovePoints)
				return;

			// we are now sure, that unit is allowed to move and target is in range
			// now performce actual move
			model.MoveUnit(unit, target);
			// trigger OnStayEffect on each topping on the path
			for(int i=0; i<path.Length; i++) {
				if(path[i].topping != null)
					path[i].topping.OnStayEffect(unit);
			}

			// clear units move resource, since only one move per turn is permitted
			unit.MovePoints = 0;
		}

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
