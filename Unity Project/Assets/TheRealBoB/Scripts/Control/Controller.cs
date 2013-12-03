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

		// TODO try to read this from an json-file
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
		new CStartTurn(model).Execute();
    }
	
	void HandleMapTileTapped(object sender, EventArgs args)
	{
		Debug.Log("Tap Event triggert");
	}
	#endregion

	public void MoveUnit(Unit unit, MapTile mapTile)
	{
		new CMoveUnit(model,unit,mapTile).Execute();
	}

	public void EndTurn()
	{
		if (model.combat.TurnsLeft () > 0)
			new CStartTurn (model).Execute ();
		else
			model.combat.SetupRound (model.units);
	}

	public byte[][] GetDistanceMatrix(Point position, int range)
	{
		byte[,] grid = model.GetGrid();
		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;
		return pathFinder.GetDistanceMatrix(position, range);
	}
}

