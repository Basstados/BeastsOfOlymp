using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithms;

public class AIHunter : IArtificalIntelligence {

	Model model;
	Controller controller;
	Unit controlledUnit;

	Unit attackTarget;

	TurnPlan myPlan;

	public AIHunter (Model model, Controller controller, Unit controlledUnit)
	{
		this.model = model;
		this.controller = controller;
		this.controlledUnit = controlledUnit;
		this.myPlan = new TurnPlan();
	}

	public TurnPlan DoPlanning ()
	{
		myPlan.attackTarget = FindTarget();
		myPlan.attack = ChooseAttack();
		myPlan.movementTarget = FindMoveDestination(myPlan.attack, myPlan.attackTarget);

		return myPlan;
	}

	/// <summary>
	/// The Hunter searches for the closed target and attacks this one until one of them dies.
	/// </summary>
	Unit FindTarget()
	{
		// for first attempt get closest enemy unit
		List<Unit> enemies = model.GetUnitsFromTeam(Unit.Team.PLAYER);
		int closedDistance;
		// if there is no old target take the first one as old target
		if(attackTarget == null || !attackTarget.Alive) {
			closedDistance = int.MaxValue;
		} else {
			// our target is still alive, so continue hunting
			return attackTarget;
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
		return attackTarget;
	}

	MapTile FindMoveDestination(Attack attack, Unit attackTarget) 
	{
		// by default we don't move (= move to the current mapTile)
		MapTile moveDestionation = controlledUnit.mapTile;

		// if all mapTiles have penalty 1 
		// this is the highes range we can attack someone this turn
		int moveRangWithAttack = controlledUnit.MovePoints;
		int maxRange = attack.range + moveRangWithAttack;
		
		int maxTargetDst = Distance(attackTarget.mapTile, controlledUnit.mapTile);
		if (maxRange > maxTargetDst) {
			// we can propably hit the target this turn
			// find the mapTile furthes away from target where we still can hit it
			Point pos = new Point(controlledUnit.mapTile.x, controlledUnit.mapTile.y);
			byte[][] dstMatrix = controller.GetDistanceMatrix(pos, moveRangWithAttack, false);
			for (int i = 0; i < dstMatrix.Length; i++) {
				for (int j = 0; j < dstMatrix[i].Length; j++) {
					if (dstMatrix[i][j] > 0) {
						// this mapTile is reachable
						int dstTarget = Distance(model.mapTiles[i][j], attackTarget.mapTile);
						if (dstTarget <= attack.range) {
							// this mapTile is close enough to the target to attack
							if (dstTarget > maxTargetDst) {
									// take the furthest mapTile (still in range)
									maxTargetDst = dstTarget;
									moveDestionation = model.mapTiles[i][j];
							}
						}
					}
				}	
			}
		}

		// if we found a new destination stop here
		if (moveDestionation != controlledUnit.mapTile)
			return moveDestionation;
		
		// we couldn't find a mapTile in range where we can hit the target
		// get as close as possible to the target
		Debug.Log("Target out of range - get closer!");

		// use the pathfinder to get closed path to enemies and cut depending on remaining Action Points
		model.UseMoveGrid();
		byte[,] grid = model.grid;
		// ignore target unit on grid so a* can get a result
		grid[attackTarget.mapTile.x, attackTarget.mapTile.y] = attackTarget.mapTile.PenaltyIgnoreUnit;

		Path path = controller.GetPath(controlledUnit.mapTile,attackTarget.mapTile,grid);
		path.DropLast();
		while(path.Cost > controlledUnit.MovePoints) {
			// remove last maptile from path until the cost are lower than action points
			path.DropLast();
		}

		return path[path.Length-1];
	}

	Attack ChooseAttack()
	{
		return controlledUnit.attacks[controlledUnit.defaultAttack];
	}


	int Distance(MapTile a, MapTile b) 
	{
		return (int) Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}
}
