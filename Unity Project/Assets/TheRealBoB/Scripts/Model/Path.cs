using System.Collections.Generic;
using System.Linq;

public struct Path  {

	MapTile[] steps;
	int cost;

	public bool Empty {
		get { return (steps == null);}
	}
	public int Length {
		get { return (steps != null) ? steps.Length : 0; }
	}

	public MapTile Last {
		get { return this[this.Length - 1];}
	}

	public MapTile First {
		get { return this[0]; }
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

	public void Add(Path path) {
		// only add paths which starts where this path ends
		if(this.Last != path.First) return;

		// convert the steps of both path to lists
		List<MapTile> myTmp = steps.ToList();
		List<MapTile> otherTmp = path.steps.ToList();
		// remove first since this one would be doubled
		otherTmp.RemoveAt(0);
		// add steps from other path to this path
		foreach(MapTile mapTile in otherTmp) {
			myTmp.Add(mapTile);
		}
		// convert back to array
		steps = myTmp.ToArray();
		// update costs
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
