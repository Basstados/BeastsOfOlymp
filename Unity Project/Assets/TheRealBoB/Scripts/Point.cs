using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Point {
	public int x;
	public int y;

	public Point(int x, int y ) 
	{
		this.x = x;
		this.y = y;
	}

	/**
	 * Calculate distance of two points with the manhatten metric
	 */ 
	public static int ManhattanDistance( Point start, Point end ) 
	{
		return Mathf.Abs(start.x - end.x) + Mathf.Abs( start.y - end.y);
	}

	public int[] ToInt()
	{
		return new int[]{x,y};
	}

	public override string ToString () 
	{
		return "("+x+","+y+")";
	}
}

