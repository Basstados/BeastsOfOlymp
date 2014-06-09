using System;
using UnityEngine;

[System.Serializable]
public class MapTile
{
	// how much is the cost to move here
	// 0 = unpasssable
	// 1 = default
	[SerializeField] byte penalty = 1;
	public byte Penalty 
	{
		get
		{
			// return 0 if topping is blocking
			if(topping !=null)
				if(topping.IsObstacle) return 0;
			// return 0 if there is a unit on this mapTile; return "penalty" otherwise
			return (unit == null) ? penalty : (byte) 0;
		}
		set
		{
			penalty = value;
		}
	}
	public byte PenaltyAttack 
	{
		get
		{
			// return 0 if topping is not targetable
			if(topping !=null)
				if(!topping.IsTargetable) return 0;

			return penalty;
		}
	}
	// which unit is sitting here right now
	[NonSerialized]
	public Unit unit;
	// what ever else may be lieing/standing here
	public Topping topping;
	// coordinates
	[HideInInspector]
	public int x;
	[HideInInspector]
	public int y;

	public MapTile(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.penalty = 1;
	}

	public override string ToString ()
	{
		return string.Format("[MapTile: Penalty={0}, PenaltyIgnoreUnit={1}, (x,y)=({2},{3})]", Penalty, PenaltyAttack, x, y);
	}
}