using System;
using System.Collections.Generic;
using Algorithms;

public class CMoveUnit : ICommand
{
	Model model;
	Unit unit;
	MapTile target;

	public CMoveUnit (Model model, Unit unit, MapTile target)
	{
		this.model = model;
		this.unit = unit;
		this.target = target;
	}


	public void Execute()
	{
		// stop if unit is not allowed to move
		if(!unit.canMove)
			return;

		byte[,] grid = model.GetGrid();
		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;

		Point start = new Point(unit.mapTile.x,unit.mapTile.y);
		Point end = new Point(target.x, target.y);

		List<PathFinderNode> result = pathFinder.FindPath(start, end);
		MapTile[] path = model.ConvertPathToMapTiles(result);
		int cost = model.GetPathCost(path);
		// stop if target is to for unit move
		if(cost > unit.MovementRange)
			return;

		// we are now sure, that unit is allowed to move and target is in range
		// now performce actual move
		model.MoveUnit(unit, target);

		// after everything is done here let's fire an event to notify others
		EventProxyManager.FireEvent(EventName.UnitMoved,this,new UnitMovedEvent(unit, path));
	}
}

public class UnitMovedEvent : System.EventArgs
{
	public Unit unit;
	public MapTile[] path;

	public UnitMovedEvent (Unit unit, MapTile[] path)
	{
		this.unit = unit;
		this.path = path;
	}
}
