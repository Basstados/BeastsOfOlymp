using System;


public class TestPoint {

	public int X;
	public int Y;
	public int penalty;

	public TestPoint (int xCoord, int yCoord, int penaltyValue) {
		X = xCoord;
		Y = yCoord;
		penalty = penaltyValue;
	}

	public TestPoint (int xCoord, int yCoord) {
		X = xCoord;
		Y = yCoord;
		penalty = 1;
	}

	public int[] ToInt() {
		return new int[2]{ X, Y};
	}

	public string ToString() {
		return "Testpoint (" + X + "," + Y + ")";
	}
}

