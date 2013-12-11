using UnityEngine;
using System;
using Algorithms;

public class Controller
{
	Model model;
	BView bView;

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
		
		model = new Model();
		model.InitMap(mapData);
		model.InitUnits(mapData);
		model.InitCombat();

		// this is just for debugging/develop builds an be removed savely
		if(Debug.isDebugBuild)
			GameObject.FindObjectOfType<DevelopPanelMaster>().Init(this, model, bView);
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
		TurnStartedEvent e = args as TurnStartedEvent;
		if(e.unit.AIControled) {
			e.unit.ai.PlanTurn();

			MoveUnit(e.unit, e.unit.ai.MoveDestionation);
			AttackUnit(e.unit, e.unit.ai.AttackTarget, e.unit.ai.AttackChoice);
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
			grid = model.GetAttackGrid();
		} else {
			grid = model.GetMoveGrid();
		}
		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;
		return pathFinder.GetDistanceMatrix(position, actionPoints);
	}
}

