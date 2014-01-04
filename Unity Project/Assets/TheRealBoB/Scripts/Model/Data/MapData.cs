using System;
using System.Collections.Generic;

[System.Serializable]
public class MapData
{
	public int width;
	public int height;
	public byte[][] penalties; // byte[width][height]; 0 means unpassable
	public TeamUnit[][] teamUnits;

	[System.Serializable]
	public class TeamUnit
	{
		public string name;
		public Point position;

		public TeamUnit() {}

		public TeamUnit (string name, Point position)
		{
			this.name = name;
			this.position = position;
		}
	}
}

