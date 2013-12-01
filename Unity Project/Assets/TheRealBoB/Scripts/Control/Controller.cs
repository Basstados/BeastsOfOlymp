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
        EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleRoundSetup);

		// TODO try to read this from an json-file
		model = new Model();
		model.InitMap(10, 10);
		model.SpawnUnit(model.mapTiles[2][2], Unit.Team.PLAYER);
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

	public void StartTurn()
	{
		new CStartTurn(model).Execute();
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

