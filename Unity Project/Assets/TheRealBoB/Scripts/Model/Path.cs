using System.Collections.Generic;
using System.Linq;

public struct Path  {

	MapTile[] steps;
	int cost;

	public bool Empty {
		get { return (steps == null);}
	}
	public int Length {
		get { return steps.Length; }
	}

	public MapTile this[int i]
	{
		get { return steps[i]; }
	}

	public int Cost 
	{
		get { return cost;}
	}

	public Path(MapTile[] steps)
	{
		this.steps = steps;
		this.cost = 0;
		CalculateCost();
	}

	public void DropLast()
	{
		List<MapTile> tmp = steps.ToList();
		tmp.RemoveAt(tmp.Count - 1);
		steps = tmp.ToArray();
		CalculateCost();
	}

	void CalculateCost() 
	{
		cost = 0;
		foreach(MapTile mapTile in steps) {
			cost += mapTile.Penalty;
		}
	}
}
