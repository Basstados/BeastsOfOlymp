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
		int damage = 0;

		// check hit chance
		if(attack.hitChance >= hit) {
			// calculate type modifier
			float typeModifier = 1f;
			if(attack.type.strengths.Length > 0)
				if(Array.Exists(attack.type.strengths, delegate(string t) { return t == target.data.type.name; })) {
					typeModifier *= 2f;
				}
			if(attack.type.weaknesses.Length > 0)
				if(Array.Exists(attack.type.weaknesses, delegate(string t) { return t == target.data.type.name; })) {
					typeModifier *= 1/2f;
				}
			EventProxyManager.FireEvent(this, new DebugLogEvent("typeModifier: " + typeModifier));

			// the actual damage formular
			damage = (int) Math.Round((source.Attack + attack.damage) * typeModifier);

			// attack is succesfull
			target.LoseHealth(damage);
			success = true;
		}

		// when target died
		if(target.HealthPoints <= 0) {
			// remove target from map
			target.mapTile.unit = null;
			// fire event
			EventProxyManager.FireEvent(this, new UnitDiedEvent(target));
		}
		EventProxyManager.FireEvent(this, new UnitAttackedEvent(attack,source,target,success, damage));
	}
}

public class UnitAttackedEvent : EventProxyArgs 
{
	public Attack attack;
	public Unit source;
	public Unit target;
	public bool hit;
	public int damage;

	public UnitAttackedEvent (Attack attack, Unit source, Unit target, bool hit, int damage)
	{
		this.name = EventName.UnitAttacked;
		this.attack = attack;
		this.source = source;
		this.target = target;
		this.hit = hit;
		this.damage = damage;
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
