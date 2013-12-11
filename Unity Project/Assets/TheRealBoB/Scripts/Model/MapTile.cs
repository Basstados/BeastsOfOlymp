using System;

[System.Serializable]
public class MapTile
{
	// how much is the cost to move here
	// 0 = unpasssable
	// 1 = default
	byte penalty;
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
}

