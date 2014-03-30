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
			return (unit == null) ? penalty : (byte) 0;
		}
		set
		{
			penalty = value;
		}
	}
	public byte PenaltyIgnoreUnit {get{return penalty;}}
	// which unit is sitting here
	[NonSerialized]
	public Unit unit;
	// coordinates
	public int x;
	public int y;

	public MapTile (int x, int y)
	{
		this.x = x;
		this.y = y;
		this.penalty = 1;
	}

	public override string ToString ()
	{
		return string.Format("[MapTile: Penalty={0}, PenaltyIgnoreUnit={1}, (x,y)=({2},{3})]", Penalty, PenaltyIgnoreUnit, x, y);
	}
}