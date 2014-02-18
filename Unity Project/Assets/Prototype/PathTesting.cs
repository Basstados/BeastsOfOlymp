using UnityEngine;
using System.Collections.Generic;
using Algorithms;

public class PathTesting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		byte[,] grid = new byte[10,10];
		for (int i = 0; i < grid.GetLength(0); i++) {
			for (int j = 0; j < grid.GetLength(1); j++) {
				grid[i,j] = 1;
			}
		}
		grid[0,3] = 0;
		grid[2,5] = 0;
		grid[4,5] = 0;
		grid[5,5] = 0;
		grid[9,4] = 0;

		PathFinder pathFinder = new PathFinder(grid);
		pathFinder.Diagonals = false;
		pathFinder.PunishChangeDirection = false;
		pathFinder.SearchLimit = 9000;
		pathFinder.DebugFoundPath = true;
		pathFinder.DebugProgress = true;


		Vector startPoint = new Vector(9,4);
		Vector endPoint = new Vector(9,3);
		Debug.Log(startPoint + " -> " + endPoint);

		List<PathFinderNode> result = pathFinder.FindPath(startPoint, endPoint);

		string str = "";
		for (int i = 0; i < grid.GetLength(0); i++) {
			for (int j = 0; j < grid.GetLength(1); j++) {
				str += grid[i,j] + " ";
			}
			str += "\n";
		}
		Debug.LogError("Grid: \n " + str);

		Debug.Log("Result: " + result.Count);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
