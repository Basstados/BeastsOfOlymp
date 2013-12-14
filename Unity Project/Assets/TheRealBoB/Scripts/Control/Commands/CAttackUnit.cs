using System;

public class CAttackUnit : ICommand
{
	Attack attack;
	Unit source;
	Unit target;
	Controller controller;

	public CAttackUnit(Unit source, Unit target, Attack attack, Controller controller)
	{
		this.source = source;
		this.attack = attack;
		this.target = target;
		this.controller = controller;
	}

	public void Execute ()
	{
		if(source.ActionPoints < attack.apCost)
			return;

		source.UseAP(attack.apCost);

		float hit = (float) new Random().NextDouble();
		bool  success = false;

		// check hit chance
		if(attack.hitChance >= hit) {
			// attack is succesfull
			target.LoseHealth(source.Attack + attack.damage);
			success = true;
		}

		// when target died
		if(target.HealthPoints <= 0) {
			// remove target from map
			target.mapTile.unit = null;
			// fire event
			EventProxyManager.FireEvent(this, new UnitDiedEvent(target));
		}
		EventProxyManager.FireEvent(this, new UnitAttackedEvent(attack,source,target,success));
	}
}

public class UnitAttackedEvent : EventProxyArgs 
{
	public Attack attack;
	public Unit source;
	public Unit target;
	public bool hit;

	public UnitAttackedEvent (Attack attack, Unit source, Unit target, bool hit)
	{
		this.name = EventName.UnitAttacked;
		this.attack = attack;
		this.source = source;
		this.target = target;
		this.hit = hit;
	}
	
}

public class UnitDiedEvent : EventProxyArgs
{
	public Unit unit;

	public UnitDiedEvent (Unit unit)
	{
		this.name = EventName.UnitDied;
		this.unit = unit;
	}
}
