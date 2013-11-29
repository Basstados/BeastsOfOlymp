using UnityEngine;
using System.Collections;

public struct Point {
	public int x;
	public int y;

	public Point(int px, int py ) {
		x = px;
		y = py;
	}

	/**
	 * Calculate distance of two points with the manhatten metric
	 */ 
	public static int ManhattanDistance( Point start, Point end ) {
		return Mathf.Abs(start.x - end.x) + Mathf.Abs( start.y - end.y);
	}

	public override string ToString () {
		return "("+x+","+y+")";
	}
}

