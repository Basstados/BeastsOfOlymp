using System;

public class LogEntryMove : ILogEntry
{
	Unit unit;
	Vector from;
	Vector to;

	public LogEntryMove (UnitMovedEvent e)
	{
		this.unit = e.unit;
		this.from = new Vector(e.path[0].x, e.path[0].y);
		this.to = new Vector(e.path[e.path.Length-1].x, e.path[e.path.Length-1].y);
	}

	public override string ToString ()
	{
		return "MOVE: " + unit.Name + "(" + unit.team + ") " + from + " --> " + to;
	}
}

