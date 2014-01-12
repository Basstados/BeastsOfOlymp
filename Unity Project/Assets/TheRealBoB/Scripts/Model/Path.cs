using System.Collections;

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
		get { return cost; }
	}

	public Path(MapTile[] steps, int cost)  {
		this.steps = steps;
		this.cost = cost;
	}
}
