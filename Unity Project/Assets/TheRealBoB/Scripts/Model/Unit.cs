using System;
using System.Collections;

[System.Serializable]
public class Unit : IComparable
{
	public string name;
	public UnitData data;
	public MapTile mapTile { get; set; }
	public Team team;
	public bool canAttack;
	public bool canMove;

	#region properties
	public int Initiative 			{get{return this.data.baseInitiative;}}
	public int Movement 	{get{return this.data.baseMovement;}}
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
		PLAYER = 0,
		AI = 1
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
			return otherUnit.Initiative.CompareTo(this.Initiative);
		else
			throw new ArgumentException("Object is not a Unit");
	}
}


