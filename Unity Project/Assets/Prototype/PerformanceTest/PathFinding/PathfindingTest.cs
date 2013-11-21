﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Algorithms;

public class PathfindingTest : MonoBehaviour {

	public Map map;
	public UILabel labelBruteforce;
	public UILabel labelAStar;

	TestPoint start;
	TestPoint goal;

	// Use this for initialization
	void Start () {


	}

	public void StartTest() {
		Quad[,] quadMatrix = map.QuadMatrix;
		byte[,] grid = new byte[quadMatrix.GetLength(0),quadMatrix.GetLength(1)];

		for( int i=0; i< quadMatrix.GetLength(0); i++ ) {
			for( int j=0; j<quadMatrix.GetLength(1); j++ ) {
				grid[i,j] = (quadMatrix[i,j].penalty > 0) ? (byte) 1 : (byte) 0;
			}
		}

		// select 2 random points on the map
		start = new TestPoint( UnityEngine.Random.Range(0, quadMatrix.GetLength(0)), UnityEngine.Random.Range(0, quadMatrix.GetLength(1)), 1);
		goal = new TestPoint( UnityEngine.Random.Range(0, quadMatrix.GetLength(0)), UnityEngine.Random.Range(0, quadMatrix.GetLength(1)), 1);

		Debug.Log("Start: " + start.ToString() );
		Debug.Log("Goal: " + goal.ToString() );
		
		// messure time while pathfinding
		float startTime = Environment.TickCount;
		List<int[]> path = map.GetShortestPath( start.ToInt(), goal.ToInt() );
		float finishTime = Environment.TickCount;
		PrintPath( path );
		// print out result
		labelBruteforce.text = "Bruteforce Time: " + (finishTime - startTime) + "ms";

		startTime = Environment.TickCount;
		PathFinder pf = new PathFinder(grid);
		List<PathFinderNode> astarPath = pf.FindPath( start, goal );
		finishTime = Environment.TickCount;
		PrintAStarPath( astarPath );
		// print out result
		labelAStar.text = "AStar Time: " + (finishTime - startTime) + "ms";
	}


	// Update is called once per frame
	void Update () {
	
	}

	void PrintPath( List<int[]> path ) {
		string str = "";
		foreach( int[] p in path ) {
			str+= "(" + p[0] + "," + p[1] + ") ";
		}
		Debug.Log("Path: " + str);
	}

	void PrintAStarPath( List<PathFinderNode> path ) {
		string str = "";
		foreach( PathFinderNode p in path ) {
			str+= "(" + p.X + "," + p.Y + ") ";
		}
		Debug.Log("AStarPath: " + str);
	}
}
