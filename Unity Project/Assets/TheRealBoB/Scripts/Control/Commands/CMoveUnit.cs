using System;
using System.Collections.Generic;
using Algorithms;

public class CMoveUnit : ICommand
{
	Model model;
	Controller controller;
	Unit unit;
	Path path;

	public CMoveUnit (Model model, Unit unit, Path path, Controller controller)
	{
		this.model = model;
		this.unit = unit;
		this.path = path;
		this.controller = controller;
	}


	public void Execute()
	{
		// stop if start and destination are the same
		if((unit.mapTile == path.Last)
			// or if target is to far away for unit move
			|| (path.Cost > unit.MovePoints))
		{
			path = new Path();
		}
		else
		{
			// we are now sure, that unit is allowed to move and target is in range
			// now performce actual move
			model.MoveUnit(unit, path.Last);
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
