using UnityEngine;
using System;
using System.Collections.Generic;
using Algorithms;

public class Controller
{
	Model model;
	BView bView;
	PathFinder pathFinder;

	public Controller (BView bView)
	{
		this.bView = bView;
		Init();
	}

	void Init() {
		Database.LoadFromFile();
		MapData mapData = Database.GetMapData();

        EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleRoundSetup);
		EventProxyManager.RegisterForEvent(EventName.UnitDied, HandleUnitDied);
		EventProxyManager.RegisterForEvent(EventName.TurnStarted, HandleTurnStarted);
		
		model = new Model(this);
		model.InitMap(mapData);
		model.InitUnits(mapData);

		// init pathFinder object
		pathFinder = new PathFinder(model.grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;

		model.InitCombat();
	}

	#region event handler
    void HandleRoundSetup(object sender, EventArgs args)
    {
        // after round setup start it
		new CStartTurn(model,this).Execute();
    }

	void HandleUnitDied (object sender, EventArgs args)
	{
		new CGameover(model).Execute();
	}
	
	void HandleMapTileTapped(object sender, EventArgs args)
	{
		Debug.Log("Tap Event triggert");
	}

	void HandleTurnStarted (object sender, EventProxyArgs args)
	{
		if(!model.matchRunning)
			return;

		TurnStartedEvent e = args as TurnStartedEvent;
		if(e.unit.AIControled) {
			TurnPlan plan = e.unit.ai.DoPlanning();

			MoveUnit(e.unit, plan.movementTarget);
			AttackUnit(e.unit, plan.attackTarget, plan.attack);
		}
	}
	#endregion

	public void MoveUnit(Unit unit, MapTile mapTile)
	{
		new CMoveUnit(model,unit,mapTile, this).Execute();
	}

	public void AttackUnit(Unit source, Unit target, Attack attack )
	{
		new CAttackUnit(source,target,attack, this).Execute();

		EndTurn();
	}

	public void EndTurn()
	{
		if (model.combat.TurnsLeft() > 0)
			new CStartTurn(model,this).Execute ();
		else
			model.combat.SetupRound (model.units);
	}

	public byte[][] GetDistanceMatrix(Point position, int actionPoints, bool ignoreUnits)
	{
		byte[,] grid;
		if(ignoreUnits) {
			model.UseAttackGrid();
		} else {
			model.UseMoveGrid();
		}
		return pathFinder.GetDistanceMatrix(position, actionPoints);
	}

	public Path GetPath(MapTile start, MapTile goal, byte[,] grid) {
		if(grid != null) {
			model.grid = grid;
		}

		// check if target mapTile is passable
		if(model.grid[goal.x,goal.y] == 0) {
			Debug.LogError("Tried to get path to a taken mapTile: " + start.ToString() + " --> " + goal.ToString());
			return new Path();
		}
		Point startPoint = new Point(start.x,start.y);
		Point endPoint = new Point(goal.x, goal.y);
		
		List<PathFinderNode> result = pathFinder.FindPath(startPoint, endPoint);

		if(result == null) {
			Debug.LogError("Caluclated path is empty; i.e. there is no path for the given parameters! \n" + start.ToString() + " --> " + goal.ToString() + "\n" + model.grid);
			string str = "";
			for (int i = 0; i < model.grid.GetLength(0); i++) {
				for (int j = 0; j < model.grid.GetLength(1); j++) {
					str += model.grid[i,j] + " ";
				}
				str += "\n";
			}
			Debug.LogError("Grid: \n " + str);
		}

		Path path = GeneratePathObject(result);


		return path;
	}

	public Path GetPath(MapTile start, MapTile goal)
	{
		model.UseMoveGrid();
		return GetPath(start, goal, null);
	}

	public Path GeneratePathObject(List<PathFinderNode> path)
	{
		MapTile[] mapTilePath = new MapTile[path.Count];
		
		for (int i = 0; i < path.Count; i++) {
			mapTilePath[i] = model.mapTiles[path[i].X][path[i].Y];	
		}

		return new Path(mapTilePath);
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
}

