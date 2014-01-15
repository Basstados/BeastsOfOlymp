using System;

public struct TurnPlan
{
	public bool shouldMove;
	public MapTile movementTarget;
	public bool shouldAttack;
	public Unit attackTarget;
	public Attack attack;

	public TurnPlan (bool shouldMove, MapTile movementTarget, bool shouldAttack, Unit attackTarget, Attack attack)
	{
		this.shouldMove = shouldMove;
		this.movementTarget = movementTarget;
		this.shouldAttack = shouldAttack;
		this.attackTarget = attackTarget;
		this.attack = attack;
	}
}