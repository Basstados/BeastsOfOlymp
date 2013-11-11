using UnityEngine;
using System.Collections;

public class Quad {

	public Vector2 position;
	public int penalty;
	
	public Quad(Vector2 pos, int pen) {
		position = pos;
		penalty = pen;
	}
	
	public override string ToString ()
	{
		return "Quad: (" + (int) position.x + ", " + (int) position.y + ")  penalty = " + penalty;
	}
}
