using System;

public class LogEntryAttack : ILogEntry
{
	Unit source;
	Unit target;
	Attack attack;
	bool hit;

	public LogEntryAttack(UnitAttackedEvent e)
	{
		this.source = e.source;
		this.target = e.target;
		this.attack = e.attack;
		this.hit = e.hit;
	}

	public override string ToString ()
	{
		int hpBefore;
		int hpAfter = target.HealthPoints;
		if(hit)
			hpBefore = target.HealthPoints + attack.damage;
		else
			hpBefore = target.HealthPoints;

		return "ATTACK: " + attack.name + "(" + attack.damage + ") " 
			+ source.Name + "(" + source.team + ") --> "
			+ target.Name + "(" + target.team + ") "
			+ "HP: " + hpAfter + "/" + hpBefore;
	}
}

