using System;
using System.Collections;

[System.Serializable]
public class Vector : Object {
	public int x;
	public int y;

	public static Vector zero { get { return new Vector(0,0); } }

	public Vector()
	{
		this.x = 0;
		this.y = 0;
	}

	public Vector(int x, int y ) 
	{
		this.x = x;
		this.y = y;
	}
	#region static
	/// <summary>
	/// Calculate distance of two points with the manhatten metric
	/// </summary>
	/// <returns>The distance.</returns>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	public static int ManhattanDistance(Vector start, Vector end) 
	{
		return Math.Abs(start.x - end.x) + Math.Abs( start.y - end.y);
	}

	/// <summary>
	/// Rotation from (1,0) to vec as rotation matrix.
	/// </summary>
	/// <returns>The rotation matrix.</returns>
	/// <param name="vec">The vector to rotate to.</param>
	public static int[,] RotateToMatrix(Vector vec)
	{
		if (vec == Vector.zero)
			vec.x = 1;
		int[,] rot = new int[2,2];
		rot[0,0] = vec.x;	rot[0,1] = - vec.y;
		rot[1,0] = vec.y;	rot[1,1] = vec.x;
		return rot;
	}
	#endregion

	/// <summary>
	/// Normalize point/vector to on of 4 unit directions.
	/// </summary>
	public void NormalizeTo4Direction()
	{
		if (Math.Abs(this.x) > Math.Abs(this.y)) {
			this.x = Math.Sign(this.x);
			this.y = 0;
		} else {
			this.x = 0;
			this.y = Math.Sign(this.y);
		}
	}

	/// <summary>
	/// Multiplie matrix from the left.
	/// </summary>
	/// <param name="matrix">Matrix</param>
	public void ApplyMatrix(int[,] matrix)
	{
		int oldX = this.x;
		int oldY = this.y;
		this.x = oldX * matrix[0,0] + oldY * matrix[0,1];
		this.y = oldX * matrix[1,0] + oldY * matrix[1,1];
	}

	/// <summary>
	/// Convert vector to int array.
	/// </summary>
	/// <returns>The int.</returns>
	public int[] ToInt()
	{
		return new int[]{x,y};
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Vector"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Vector"/>.</returns>
	public override string ToString () 
	{
		return "("+x+","+y+")";
	}

	/// <summary>
	/// Clone this instance.
	/// </summary>
	public Vector Clone()
	{
		return new Vector(this.x, this.y);
	}

	public override bool Equals (object obj)
	{
		Vector other = (Vector) obj;
		return (this.x == other.x && this.y == other.y);
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public static bool operator ==(Vector v1, Vector v2)
	{
		return v1.Equals(v2);
	}

	public static bool operator !=(Vector v1, Vector v2)
	{
		return !v1.Equals(v2);
	}
}

