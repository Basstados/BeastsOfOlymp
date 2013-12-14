using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithms;

public class ArtificalIntelligence : MonoBehaviour {

	Model model;
	Unit controlledUnit;

	// result of the decisions of the current turn
	Unit attackTarget;
	public Unit AttackTarget {get{return attackTarget;}}
	MapTile moveDestionation;
	public MapTile MoveDestionation {get{return moveDestionation;}}
	Attack attackChoice;
	public Attack AttackChoice {get{return attackChoice;}}

	public ArtificalIntelligence (Model model, Unit controlledUnit)
	{
		this.model = model;
		this.controlledUnit = controlledUnit;
	}
	
	public void PlanTurn()
	{
		if(model.matchRunning) {
			//TODO maybe implement GOAP AI for more intelligent planning
			ChooseTarget();
			FindMoveDestination();
			ChooseAttack();
		}
	}

	void ChooseTarget()
	{
		// for first attempt get closest enemy unit
		List<Unit> enemies = model.GetUnitsFromTeam(Unit.Team.PLAYER);
		int closedDistance;
		// if there is no old target take the first one as old target
		if(attackTarget == null || !attackTarget.Alive) {
			closedDistance = int.MaxValue;
		} else {
			// set distance to old target
			closedDistance = Mathf.Abs(attackTarget.mapTile.x - controlledUnit.mapTile.x) + Mathf.Abs(attackTarget.mapTile.y - controlledUnit.mapTile.y);
		}

		// get closed enemie unit
		foreach(Unit unit in enemies) {
			if(!unit.Alive)
				continue;

			// TODO use A* for real distance value and not just exspectation
			int dist = Mathf.Abs(unit.mapTile.x - controlledUnit.mapTile.x) + Mathf.Abs(unit.mapTile.y - controlledUnit.mapTile.y);
			if(dist < closedDistance) {
				closedDistance = dist;
				attackTarget = unit;
			}

		}

		// attackTarget was updated
	}

	void FindMoveDestination() 
	{
		// use the pathfinder to get closed path to enemies and cut depending on remaining Action Points
		byte[,] grid = model.GetMoveGrid();
		// ignore target unit on grid so a* can get a result
		grid[AttackTarget.mapTile.x, AttackTarget.mapTile.y] = AttackTarget.mapTile.PenaltyIgnoreUnit;

		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;
		
		Point start = new Point(controlledUnit.mapTile.x,controlledUnit.mapTile.y);
		Point end = new Point(AttackTarget.mapTile.x, AttackTarget.mapTile.y);
		
		List<PathFinderNode> result = pathFinder.FindPath(start, end);
		// drop last since its the maptile the attack target is sitting on
		result.RemoveAt(result.Count-1);

		MapTile[] path = model.ConvertPathToMapTiles(result);
		int cost = model.GetPathCost(path);
		while(cost > controlledUnit.ActionPoints) {
			// remove last maptile from path until the cost are lower than action points
			List<MapTile> tmp = path.ToList();
			tmp.RemoveAt(tmp.Count-1);
			path = tmp.ToArray();
			cost = model.GetPathCost(path);
		}

		moveDestionation = path.Last();
	}

	void ChooseAttack()
	{
		attackChoice = controlledUnit.attacks[controlledUnit.defaultAttack];
	}

}
