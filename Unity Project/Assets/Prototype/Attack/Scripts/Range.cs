using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Range {
	
	public int intValue;
	int[,] distanceMatrix;
	int[] currentPos;
	
	BattlefieldQuad[,] quadMatrix;
	
	public Range(BattlefieldQuad[,] matrix, int range) {
		quadMatrix = matrix;
		intValue = range;
	}
	
	public void UpdateCalculations( int[] position ) {
		currentPos = position;
		CalculateDistanceMatrix();
	}
	
	
	private void InitDistanceMatrix() {
		distanceMatrix = new int[quadMatrix.GetLength(0), quadMatrix.GetLength(1)];
		// set all values to int.MaxValue (for unreachable) as default
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for( int j=0; j<distanceMatrix.GetLength(1); j++) {
				distanceMatrix[i,j] = int.MaxValue;
			}
		}
	}
	
	private void CalculateDistanceMatrix() {
		// set distances to int.MaxValue as default
		InitDistanceMatrix();
		
		// distance to current position is always 0
		distanceMatrix[currentPos[0], currentPos[1]] = 0;
		// start recursiv distance calulation
		CheckNeighbourQuad(currentPos[0],currentPos[1],0);
	}
	
	/**
	 * [i,j] are the indices of the previous field
	 */ 
	private void CheckNeighbourQuad(int i, int j, int distance) {
		
		// neighbour quad indicies
		List<int[]> neighbour = GetNeighbourList(i,j);
		
		// check all neighbours if there are in range and continue recursion
		for( int k=0; k<neighbour.Count; k++) {
			int currValue = distanceMatrix[neighbour[k][0], neighbour[k][1]];
			int penalty = quadMatrix[neighbour[k][0], neighbour[k][1]].GetComponent<BattlefieldQuad>().movementPenalty;
			
			if( currValue > distance && intValue >= distance + penalty ) {
				// we are still in range and doesn't have a shorter way to this quad
				// so continue recursion if quad is passable (penalty > 0)
				if( penalty > 0 ) {
					distanceMatrix[neighbour[k][0], neighbour[k][1]] = distance + penalty;
					//quadMatrix[neighbour[k][0], neighbour[k][1]].renderer.material = reachableMat;
					
					CheckNeighbourQuad(neighbour[k][0], neighbour[k][1], distance + penalty);
				}	
			}
		}
	}
	
	private List<int[]> GetNeighbourList(int i, int j) {
		List<int[]> neighbour = new List<int[]>();
		
		if( j+1 < distanceMatrix.GetLength(1) ) {
			neighbour.Add(new int[2]{i, j+1});	
		}
		if( i+1 < distanceMatrix.GetLength(0) ) {
			neighbour.Add(new int[2]{i+1, j});	
		}
		if( j-1 >= 0 ) {
			neighbour.Add(new int[2]{i, j-1});	
		}
		if( i-1 >=0 ) {
			neighbour.Add(new int[2]{i-1,j});
		}
		
		return neighbour;
	}

	public int[,] DistanceMatrix {
		get {
			return this.distanceMatrix;
		}
	}
	
	public bool IsInRange( int[] target ) {
		return distanceMatrix[target[0], target[1]] <= intValue;
	}
}
