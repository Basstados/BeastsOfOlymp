using System;
using System.Collections;

[System.Serializable]
public class Unit : IComparable
{
	public string name;
	public UnitBaseStats baseStats;
	public MapTile mapTile { get; set; }

	public int Speed 
	{ 
		get 
		{
			return this.baseStats.speed;
		}
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
}


