using System;

[System.Serializable]
public class MapTile
{
	// how much is the cost to move here
	// 0 = unpasssable
	// 1 = default
	public byte penalty;
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

