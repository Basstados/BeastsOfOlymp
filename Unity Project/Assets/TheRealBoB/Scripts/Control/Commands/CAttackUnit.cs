using System;
using System.Collections.Generic;

public class CAttackUnit : ICommand
{
	Attack attack;
	Unit source;
	MapTile target;
	Model model;
	Controller controller;

	public CAttackUnit(Unit source, MapTile target, Attack attack, Model model, Controller controller)
	{
		this.source = source;
		this.attack = attack;
		this.target = target;
		this.model = model;
		this.controller = controller;
	}

	public void Execute ()
	{
		if (attack.range == 0) {
			target = source.mapTile;
		}
		// quite early if source unit is not allowed to attack or target is out of range
		if(!source.CanAttack || AttackDistance(source.mapTile, target) > attack.range)
			return;
		
		// lower attack resource on source unit
		source.AttackPoints = 0;

		float hit = (float) new Random().NextDouble();
		bool  success = false;
		int damage = 0;
		byte efficency = 1;
		float typeModifier = 1;

		// check hit chance
		if(attack.hitChance >= hit) {
			// attack is succesfull
			// apply damage to all unit in attack area
			success = true;
			int x = 0;
			int y = 0;
			List<Unit> targets = new List<Unit>();
			int[,] rot = AttackRotationMatrix(target, source);
			foreach(Point pt in attack.area) {
				Point rotPt = RotateAroundOrigin(pt, rot);
				x = target.x + rotPt.x;
				y = target.y + rotPt.y;
				if(model.IsPointOnGrid(new Point(x,y)))
					if(model.mapTiles[x][y].unit != null) {
						Unit unit = model.mapTiles[x][y].unit;

						typeModifier = CalcTypeModifier(unit);
						efficency = (byte) ((typeModifier > 1f) ? 2 : (typeModifier == 1f) ? 1 : 0);
						damage = CalcDamage(source.Attack, attack.damage, typeModifier);


						unit.LoseHealth(damage);
						targets.Add(unit);
					}
			}
			EventProxyManager.FireEvent(this, new UnitAttackedEvent(attack,source,targets,efficency, damage));

			// when target died fire event AFTER attack was performed
			foreach(Unit u in targets) {
				if(u.HealthPoints <= 0) {
					// remove target from map
					u.mapTile.unit = null;
					// fire event
					EventProxyManager.FireEvent(this, new UnitDiedEvent(u));
				}
			}
		}
	}

	/// <summary>
	/// Calculate rotation matrix you have to apply to the attack area 
	/// depending on attacker orientation
	/// </summary>
	/// <returns>The rotation matrix.</returns>
	/// <param name="target">MapTile this attack is targeting</param>
	/// <param name="source">Unit who executes the attack</param>
	private int[,] AttackRotationMatrix(MapTile target, Unit source)
	{
		// calculate a orientation of the unit using the max-value and its sign
		// we want a signed unit vector
		Point orientationVec = new Point(target.x - source.mapTile.x, target.y - source.mapTile.y);
		if(orientationVec.x == 0 && orientationVec.y == 0)
			orientationVec.x = 1;
		
		if (Math.Abs(orientationVec.x) > Math.Abs(orientationVec.y)) {
			orientationVec = new Point((int) Math.Sign(orientationVec.x),0);
		} else {
			orientationVec = new Point(0,(int) Math.Sign(orientationVec.y));
		}
		int[,] rot = new int[2,2];
		rot[0,0] = orientationVec.y;	rot[0,1] = orientationVec.x;
		rot[1,0] = - orientationVec.x;	rot[1,1] = orientationVec.y;
		return rot;
	}
	
	/// <summary>
	/// Apply given rotation matrix to an point. This give you a rotation around the origin.
	/// </summary>
	/// <returns>Rotated point (vector)</returns>
	/// <param name="pt">Point.</param>
	/// <param name="rot">Rotation matrix (2x2)</param>
	private Point RotateAroundOrigin(Point pt, int[,] rot)
	{
		return new Point(pt.x * rot[0,0] + pt.y * rot[0,1], pt.x * rot[1,0] + pt.y * rot[1,1]);
	}

	private int AttackDistance(MapTile from, MapTile to)
	{
		return Math.Abs (from.x - to.x) + Math.Abs (from.y - to.y);
	}

	private float CalcTypeModifier(Unit unit)
	{
		// calculate type modifier
		float typeModifier = 1f;
		if(attack.type.strengths.Length > 0)
		if(Array.Exists(attack.type.strengths, delegate(string t) { return t == unit.data.type.name; })) {
			typeModifier *= 2f;
		}
		if(attack.type.weaknesses.Length > 0)
		if(Array.Exists(attack.type.weaknesses, delegate(string t) { return t == unit.data.type.name; })) {
			typeModifier *= 1/2f;
		}
		return typeModifier;
	}

	private int CalcDamage(int sourceAtkValue, int atkDmg, float typeModifier) {
		// the actual damage formular
		return (int) Math.Round((sourceAtkValue + atkDmg) * typeModifier);
	}
}

public class UnitAttackedEvent : EventProxyArgs 
{
	public Attack attack;
	public Unit source;
	public List<Unit> targets;
	public byte efficieny;
	public int damage;

	public UnitAttackedEvent (Attack attack, Unit source, List<Unit> targets, byte efficieny, int damage)
	{
		this.name = EventName.UnitAttacked;
		this.attack = attack;
		this.source = source;
		this.targets = targets;
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
