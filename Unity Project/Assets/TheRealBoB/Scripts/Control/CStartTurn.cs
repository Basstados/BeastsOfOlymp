using System;

public class CStartTurn : ICommand
{
	Model model;

	public CStartTurn (Model model)
	{
		this.model = model;
	}

	public void Execute()
	{
		Unit unit = model.combat.GetNextUnit();
		model.ActivateUnit(unit);
		EventProxyManager.FireEvent(EventName.TurnStarted,this,new TurnStartedEvent(unit));
	}
}

public class TurnStartedEvent : System.EventArgs
{
	public Unit unit;

	public TurnStartedEvent (Unit unit)
	{
		this.unit = unit;
	}
}