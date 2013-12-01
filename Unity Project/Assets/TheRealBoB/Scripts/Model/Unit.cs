using System;
using System.Collections;

[System.Serializable]
public class Unit : IComparable
{
	public string name;
	public UnitBaseStats baseStats;
	public MapTile mapTile { get; set; }
	public Team team;
	public bool canAttack;
	public bool canMove;

	#region properties
	public int Speed 			{get{return this.baseStats.speed;}}
	public int MovementRange 	{get{return this.baseStats.movementRange;}}
	#endregion

	public bool AIControled
	{
		get
		{
			return team == Team.AI;
		}
	}

	public enum Team 
	{
		PLAYER,
		AI
	}

	public void ResetTurn()
	{
		canMove = true;
		canAttack = true;

	}

	/**
	 * Unit will be compared by there speed value
	 */ 
	public int CompareTo(object obj) 
	{
		if (obj == null)
			return 1;

		Unit otherUnit = obj as Unit;
		if (otherUnit != null) 
			return this.Speed.CompareTo(otherUnit.Speed);
		else
			throw new ArgumentException("Object is not a Unit");
	}

	public override string ToString ()
	{
		return string.Format ("[Unit: name={0}, baseStats={1}, team={2}, canAttack={3}, canMove={4}, mapTile={5}]", name, baseStats, team, canAttack, canMove, mapTile);
	}
	
}


