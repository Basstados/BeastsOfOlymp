using System.Collections;

class CStartRound : ICommand
{
    Model model;

	public CStartRound (Model model)
	{
		this.model = model;
	}

	public void Execute() 
	{
		Unit unit = model.combat.GetNextUnit();
		model.ActivateUnit(unit);
	}
}

