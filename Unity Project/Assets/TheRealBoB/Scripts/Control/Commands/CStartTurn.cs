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
		Unit unit = model.combat.GetNextUnit ();

		if(!unit.Alive) {
			controller.EndTurn();
			return;
		}
		// increase turn counter
		model.combat.turn++;
		// activate next unit
		model.ActivateUnit(unit);
		EventProxyManager.FireEvent (this, new TurnStartedEvent (unit, model.combat.round, model.combat.turn));
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