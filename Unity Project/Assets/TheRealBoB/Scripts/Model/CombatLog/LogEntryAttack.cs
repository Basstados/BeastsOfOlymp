using System;
using System.Collections.Generic;

public class LogEntryAttack : ILogEntry
{
	Unit source;
	List<Unit> targets;
	Attack attack;
	bool hit;

	public LogEntryAttack(UnitAttackedEvent e)
	{
		this.source = e.source;
		this.targets = e.victims;
		this.attack = e.attack;
		this.hit = (e.damage > 0);
	}

	public override string ToString ()
	{
		string str = "ATTACK: " + attack.name + "(" + attack.damage + ") " 
			+ source.Name + "(" + source.team + ") --> ";

		foreach(Unit u in targets) {
			str += u.Name + "(" + u.team + ") ";
		}

		return str;
	}
}

