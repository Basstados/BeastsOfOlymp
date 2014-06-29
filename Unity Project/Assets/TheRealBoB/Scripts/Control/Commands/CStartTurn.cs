using System;

public class CStartTurn : ICommand
{
	Model model;
	Controller controller;

	public CStartTurn (Model model, Controller controller)
	{
		this.model = model;
		this.controller = controller;
	}

	public void Execute()
	{
		// get next unit from queue
		Unit unit = model.combat.GetNextUnit ();

		// increase turn counter
		model.combat.turn++;

		// immedialty end turn if unit is already dead
		if(!unit.Alive) {
			controller.EndTurn();
			return;
		}

		// execute field effect for all unit
		foreach(Unit u in model.units) {
			if(u.mapTile.topping != null)
				u.mapTile.topping.OnStayEffect(u);
		}

		model.combat.CheckForDeadUnits();

		// check again if this unit is dead (might have died from field effects)
		if(!unit.Alive) {
			controller.EndTurn();
			return;
		}

		// activate next unit
		model.ActivateUnit(unit);
		EventProxyManager.FireEvent(this, new TurnStartedEvent (unit, model.combat.round, model.combat.turn));
	}
}

public class TurnStartedEvent : EventProxyArgs
{
	public Unit unit;
	public int round;
	public int turn;

	public TurnStartedEvent (Unit unit, int round, int turn)
	{
		this.name = EventName.TurnStarted;
		this.unit = unit;
		this.round = round;
		this.turn = turn;
	}
}