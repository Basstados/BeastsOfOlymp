/// <summary>
/// Context and manager for the game logic
/// </summary>
using UnityEngine;
using System;
using System.Collections.Generic;
using Algorithms;

public class Controller
{
	Model model;
	BView bView;
	PathFinder pathFinder;

	public Controller (BView bView, MapTile[][] mapTiles, Unit[] units)
	{
		this.bView = bView;
		Init(mapTiles, units);
	}

	void Init(MapTile[][] mapTiles, Unit[] units) 
	{
		// load database
		//GameData.LoadFromFile();

		// register for logic important events
        EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleRoundSetup);
		EventProxyManager.RegisterForEvent(EventName.UnitDied, HandleUnitDied);
		EventProxyManager.RegisterForEvent(EventName.TurnStarted, HandleTurnStarted);
		// init model
		model = new Model(this);
		model.InitMap(mapTiles);
		model.InitUnits(units);

		// init pathFinder object
		pathFinder = new PathFinder(model.grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;
		pathFinder.SearchLimit = 9000;
		pathFinder.DebugFoundPath = true;
		pathFinder.DebugProgress = true;
	}

	public void StartMatch()
	{
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

		// perform AI planning and execute it if the active unit is AI controled
		TurnStartedEvent e = args as TurnStartedEvent;
		if(e.unit.AIControled) {
			TurnPlan plan = e.unit.ai.DoPlanning();
			// execute turn plan
			MoveUnit(e.unit, plan.movementTarget);
			AttackMapTile(e.unit, plan.attackTarget.mapTile, plan.attack);
		}
	}
	#endregion

	/// <summary>
	/// Execute command for moving a unit
	/// </summary>
	/// <param name="unit">Unit to move</param>
	/// <param name="mapTile">Map tile to move to</param>
	public void MoveUnit(Unit unit, MapTile mapTile)
	{
		new CMoveUnit(model,unit,mapTile, this).Execute();
	}

	/// <summary>
	/// Execute command for attacking the given maptile and end the current turn
	/// </summary>
	/// <param name="source">Unit who is performing the attack</param>
	/// <param name="target">Target maptile</param>
	/// <param name="attack">Attack which will be performed if valid</param>
	public void AttackMapTile(Unit source, MapTile target, Attack attack )
	{
		new CAttackUnit(source,target,attack, model, this).Execute();

		EndTurn();
	}

	/// <summary>
	/// Ether start turn for next unit in queue or start new round
	/// </summary>
	public void EndTurn()
	{
		if (model.combat.TurnsLeft() > 0)
			new CStartTurn(model,this).Execute();
		else
			model.combat.SetupRound();
	}

	/// <summary>
	/// Calculate a matrix whichs tells you the shortest distance from each grid field (in range) to the start position.
	/// </summary>
	/// <returns>The distance matrix.</returns>
	/// <param name="position">Start position for the distance matrix</param>
	/// <param name="range">Range how far the distance matrix should be calculated</param>
	/// <param name="ignoreUnits">If set to <c>true</c> ignore units.</param>
	public byte[][] GetDistanceMatrix(Vector position, int range, bool ignoreUnits)
	{
		if(ignoreUnits) {
			model.UseAttackGrid();
		} else {
			model.UseMoveGrid();
		}
		return pathFinder.GetDistanceMatrix(position, range);
	}

	/// <summary>
	/// Calculate the shortest path between two given mapTiles for a certain grid
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="start">Start.</param>
	/// <param name="goal">Goal.</param>
	/// <param name="grid">Grid.</param>
	public Path GetPath(MapTile start, MapTile goal, byte[,] grid) {
		if(grid != null) {
			model.grid = grid;
		}

		// check if target mapTile is passable
		if(model.grid[goal.x,goal.y] == 0) {
			Debug.LogError("Tried to get path to a taken mapTile: " + start.ToString() + " --> " + goal.ToString());
			return new Path();
		}
		Vector startPoint = new Vector(start.x,start.y);
		Vector endPoint = new Vector(goal.x, goal.y);
		// performe actual pathfinding (A*)
		List<PathFinderNode> result = pathFinder.FindPath(startPoint, endPoint);

		// print error if no path was found
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
		// convert to easy-usable path object
		Path path = GeneratePathObject(result);
		return path;
	}

	/// <summary>
	/// Gets the shortest path using the default grid of the model class
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="start">Start.</param>
	/// <param name="goal">Goal.</param>
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

	/// <summary>
	/// Sum up the penalties of each mapTile of an path
	/// </summary>
	/// <returns>The path cost.</returns>
	/// <param name="path">Path.</param>
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

