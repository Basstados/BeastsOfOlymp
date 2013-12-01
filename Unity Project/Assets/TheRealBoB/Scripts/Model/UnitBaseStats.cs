using System;

public struct UnitBaseStats
{
	public int maxHealth;
	public int attack;
	public int movementRange;
	public int speed;

	public override string ToString ()
	{
		return string.Format ("[UnitBaseStats: maxHealth={0}, attack={1}, movementRange={2}, speed={3}]", maxHealth, attack, movementRange, speed);
	}
}



