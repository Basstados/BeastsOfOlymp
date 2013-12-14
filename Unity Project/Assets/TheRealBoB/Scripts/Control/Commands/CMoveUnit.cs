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
		byte[,] grid = model.GetMoveGrid();

		// check if target mapTile is passable
		if(grid[target.x,target.y] == 0)
			return;

		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;

		Point start = new Point(unit.mapTile.x,unit.mapTile.y);
		Point end = new Point(target.x, target.y);

		List<PathFinderNode> result = pathFinder.FindPath(start, end);
		MapTile[] path = model.ConvertPathToMapTiles(result);
		int cost = model.GetPathCost(path);
		// stop if target is to for unit move
		if(cost > unit.ActionPoints)
			return;

		// we are now sure, that unit is allowed to move and target is in range
		// now performce actual move
		model.MoveUnit(unit, target);
		unit.UseAP(cost);
		unit.CanMove = false;
		
		// after movement fire event
		EventProxyManager.FireEvent(this,new UnitMovedEvent(unit, path));
	}
}

public class UnitMovedEvent : EventProxyArgs
{
	public Unit unit;
	public MapTile[] path;

	public UnitMovedEvent (Unit unit, MapTile[] path)
	{
		this.name = EventName.UnitMoved;
		this.unit = unit;
		this.path = path;
	}
}
