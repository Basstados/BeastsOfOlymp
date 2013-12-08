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

		model.ActivateUnit(unit);
		EventProxyManager.FireEvent (this, new TurnStartedEvent (unit));
	}
}

public class TurnStartedEvent : EventProxyArgs
{
	public Unit unit;

	public TurnStartedEvent (Unit unit)
	{
		this.name = EventName.TurnStarted;
		this.unit = unit;
	}
}