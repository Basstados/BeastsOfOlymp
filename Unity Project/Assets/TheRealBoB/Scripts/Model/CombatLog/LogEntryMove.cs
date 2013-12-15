using System;

public class LogEntryMove : ILogEntry
{
	Unit unit;
	Point from;
	Point to;

	public LogEntryMove (UnitMovedEvent e)
	{
		this.unit = e.unit;
		this.from = new Point(e.path[0].x, e.path[0].y);
		this.to = new Point(e.path[e.path.Length-1].x, e.path[e.path.Length-1].y);
	}

	public override string ToString ()
	{
		return "MOVE: " + unit.Name + "(" + unit.team + ") " + from + " --> " + to;
	}
}

