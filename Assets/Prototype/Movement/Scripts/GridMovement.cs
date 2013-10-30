using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMovement : MonoBehaviour {
	
	public Material defaultMat;
	public Material reachableMat;
	public int range = 5;
	
	int[,] penaltyMatrix;
	GameObject[,] quadMatrix;
	int[,] distanceMatrix;
	
	int[] currentPos = new int[2];
	
	// Use this for initialization
	void Start () {
		penaltyMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().PenaltyMatrix;
		quadMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().QuadMatrix;
		
		Vector3 relativLevelPos = GameObject.Find("LevelGenerator").transform.TransformPoint( transform.position );
		currentPos[0] = (int) relativLevelPos.x;
		currentPos[1] = (int) relativLevelPos.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			Ray cursorRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit[] hitList = Physics.RaycastAll( cursorRay );
			
			foreach( RaycastHit hit in hitList ) {
				if( hit.collider.CompareTag("Player") ) {
					// user clicked a player go, yeah!
					Debug.Log("You clicked me!");
					DisplayRange();
					break;
				}
			}
		}
	}
	
	private void InitDistanceMatrix() {
		penaltyMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().PenaltyMatrix;
		quadMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().QuadMatrix;
		distanceMatrix = new int[penaltyMatrix.GetLength(0), penaltyMatrix.GetLength(1)];
		// set all values to int.MaxValue (for unreachable) as default
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for( int j=0; j<distanceMatrix.GetLength(1); j++) {
				distanceMatrix[i,j] = int.MaxValue;
			}
		}
	}
	
	private void DisplayRange() {
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
		Debug.Log("Checking...");
		
		// neighbour quad indicies
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
		
		// check all neighbours if there are in range and continue recursion
		for( int k=0; k<neighbour.Count; k++) {
			int currValue = distanceMatrix[neighbour[k][0], neighbour[k][1]];
			int penalty = quadMatrix[neighbour[k][0], neighbour[k][1]].GetComponent<BattlefieldQuad>().movementPenalty;
			
			if( currValue > distance && range > distance + penalty ) {
				// we are still in range and doesn't have a shorter way to this quad
				// so continue recursion if quad is passable (penalty > 0)
				if( penalty > 0 ) {
					distanceMatrix[neighbour[k][0], neighbour[k][1]] = distance + penalty;
					quadMatrix[neighbour[k][0], neighbour[k][1]].renderer.material = reachableMat;
					Debug.Log("Continue with: " + quadMatrix[neighbour[k][0], neighbour[k][1]].name);
					
					
					CheckNeighbourQuad(neighbour[k][0], neighbour[k][1], distance + penalty);
				}	
			}
		}
	}
}
