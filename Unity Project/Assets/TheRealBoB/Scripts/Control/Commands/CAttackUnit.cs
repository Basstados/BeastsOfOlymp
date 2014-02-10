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
		// quite early if source unit is not allowed to attack or target is out of range
		if(!source.CanAttack || AttackDistance(source.mapTile, target.mapTile) > attack.range)
			return;
		
		// lower attack resource on source unit
		source.AttackPoints = 0;

		float hit = (float) new Random().NextDouble();
		bool  success = false;
		int damage = 0;
		byte efficency = 1;

		// check hit chance
		if(attack.hitChance >= hit) {
			// calculate type modifier
			float typeModifier = 1f;
			if(attack.type.strengths.Length > 0)
				if(Array.Exists(attack.type.strengths, delegate(string t) { return t == target.data.type.name; })) {
					typeModifier *= 2f;
					efficency = 2;
				}
			if(attack.type.weaknesses.Length > 0)
				if(Array.Exists(attack.type.weaknesses, delegate(string t) { return t == target.data.type.name; })) {
					typeModifier *= 1/2f;
				efficency = 0;
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
		EventProxyManager.FireEvent(this, new UnitAttackedEvent(attack,source,target,efficency, damage));
	}

	private int AttackDistance(MapTile from, MapTile to)
	{
		return Math.Abs (from.x - to.x) + Math.Abs (from.y - to.y);
	}
}

public class UnitAttackedEvent : EventProxyArgs 
{
	public Attack attack;
	public Unit source;
	public Unit target;
	public byte efficieny;
	public int damage;

	public UnitAttackedEvent (Attack attack, Unit source, Unit target, byte efficieny, int damage)
	{
		this.name = EventName.UnitAttacked;
		this.attack = attack;
		this.source = source;
		this.target = target;
		this.efficieny = efficieny;
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
