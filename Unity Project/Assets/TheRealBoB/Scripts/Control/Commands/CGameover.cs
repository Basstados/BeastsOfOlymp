using System;

public class CGameover : ICommand
{
	Model model;

	public CGameover (Model model)
	{
		this.model = model;
	}

	public void Execute ()
	{
		// assume we have only 2 teams on the field
		int aliveTeamA = 0;
		int aliveTeamB = 0;
		// count number of units alive on each team
		foreach(Unit unit in model.units) {
			switch (unit.team) {
			case Unit.Team.PLAYER:
				if(unit.Alive)
					aliveTeamA++;
				break;
			case Unit.Team.AI:
				if(unit.Alive)
					aliveTeamB++;
				break;
			}
		}
		// if all units on one team are that the game is over
		bool playerDefeated = false;
		bool aiDefeated = false;
		if(aliveTeamA == 0)
			playerDefeated = true;
		if(aliveTeamB == 0)
			aiDefeated = true;

		if(playerDefeated || aiDefeated) {
			model.matchRunning = false;
			EventProxyManager.FireEvent(this, new GameoverEvent(playerDefeated,aiDefeated));
		}
	}
}

public class GameoverEvent : EventProxyArgs
{
	public bool playerDefeated;
	public bool aiDefeated;

	public GameoverEvent (bool playerDefeated, bool aiDefeated)
	{
		this.name = EventName.Gameover;
		this.playerDefeated = playerDefeated;
		this.aiDefeated = aiDefeated;
	}
}

