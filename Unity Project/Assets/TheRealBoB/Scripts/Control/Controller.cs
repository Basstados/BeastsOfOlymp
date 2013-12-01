using UnityEngine;

using System;

public class Controller
{
	Model model;

	public Controller ()
	{
		Init();
	}

	void Init() {
		EventProxyManager.RegisterForEvent(EventName.MapTileTapped, HandleMapTileTapped);
        EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleRoundSetup);
		EventProxyManager.RegisterForEvent(EventName.UnitActivated, HandleUnitActivated);

		// TODO try to read this from an json-file
		model = new Model();
		model.InitMap(10, 10);
		model.SpawnUnit(model.mapTiles[5][5], Unit.Team.PLAYER);
		model.InitCombat();
	}

    void HandleRoundSetup(object sender, EventArgs args)
    {
        // after round setup start it
		new CStartRound(model).Execute();
    }

	void HandleUnitActivated (object sender, EventArgs args)
	{
		UnitActivatedEvent e = args as UnitActivatedEvent;

		if(e.unit.AIControled) {
			// start AI
		} else {
			// players turn
			EventProxyManager.FireEvent(EventName.PlayersTurnStarted, this, args);
		}

	}
	
	void HandleMapTileTapped(object sender, EventArgs args)
	{
		Debug.Log("Tap Event triggert");
	}
}

