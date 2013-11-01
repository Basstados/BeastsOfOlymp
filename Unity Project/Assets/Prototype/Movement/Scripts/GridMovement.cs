using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMovement : MonoBehaviour {
	
	public Material defaultMat;
	public Material reachableMat;
	public Material pathMat;
	public int range = 5;
	
	int[,] penaltyMatrix;
	GameObject[,] quadMatrix;
	int[,] distanceMatrix;
	
	int[] currentPos = new int[2];
	
	bool pathSelected = false;
	List<int[]> path = new List<int[]>();
	
	private GridMoveAnimation moveAnimation;
	
	// Use this for initialization
	void Start () {
		penaltyMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().PenaltyMatrix;
		quadMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().QuadMatrix;
		
		Vector3 relativLevelPos = GameObject.Find("LevelGenerator").transform.TransformPoint( transform.position );
		currentPos[0] = (int) relativLevelPos.x;
		currentPos[1] = (int) relativLevelPos.y;
		
		moveAnimation = GetComponent<GridMoveAnimation>();
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
					CalculateDistanceMatrix();
					DisplayRange();
					break;
				}
				
				if( hit.collider.CompareTag("GridQuad") ) {
					int i = (int) hit.collider.transform.localPosition.x;
					int j = (int) hit.collider.transform.localPosition.z;
					
					// secound time we clicked this quead?
					// if yes, move to it
					if( path.Count > 0 ) {
						Debug.Log("("+i+","+j+") --- (" + path[path.Count - 1][0] + "," + path[path.Count - 1][1] + ")" );
						if( path[path.Count - 1][0] == i && path[path.Count - 1][1] == j ) {
							// path destination and clicked field are equal
							
							// use X and Z coordinate of quad for neq character position
							/*Vector3 pos = transform.position;
							pos.x = quadMatrix[i,j].transform.position.x;
							pos.z = quadMatrix[i,j].transform.position.z;
							transform.position = pos;*/
							Vector3[] wps = GetWaypoints();
							moveAnimation.StartMoveAnimation(wps);
							
							// update currentPos
							currentPos = new int[]{i,j};
							
							ClearDisplay();
						}
					}
					
					if( quadMatrix[i,j].renderer.sharedMaterial == reachableMat ) {
						// we clicked a field we can move to :D
						
						// overwride previous path
						DisplayRange();
						// calculate new path
						CalculatePath(new int[]{i,j});
					}
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
	
	private void CalculatePath(int[] currentQuad) {
		Debug.Log("Calculate path...");
		List<int[]> neighbour;
		int distanceMin;
		int[] minNeighbour = currentQuad;
		path = new List<int[]>();
		
		// walk backwards from destination to character position
		while( distanceMatrix[currentQuad[0], currentQuad[1]] > 0 ) {
			// udpate neighbour list
			neighbour = GetNeighbourList(currentQuad[0], currentQuad[1]);
			
			// find neighbour with lowest distance
			distanceMin = int.MaxValue;
			for( int k=0; k<neighbour.Count; k++ ) {
				if( distanceMatrix[neighbour[k][0], neighbour[k][1]] < distanceMin ) {
					distanceMin = distanceMatrix[neighbour[k][0], neighbour[k][1]];
					minNeighbour = neighbour[k];
				}
			}
			
			if( minNeighbour == currentQuad ) {
				Debug.LogError("Distance Matrix is wrong!");	
			}
			
			// color current quad
			quadMatrix[currentQuad[0],currentQuad[1]].renderer.material = pathMat;
			// add current quad to path
			path.Insert(0, currentQuad);
			// continue with minNeighbour
			currentQuad = minNeighbour;
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
	
	private void DisplayRange() {
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for(int j=0; j<distanceMatrix.GetLength(1); j++) {
				if( distanceMatrix[i,j] < range ) {
					// this quad is in range
					quadMatrix[i,j].renderer.material = reachableMat;
				}
			}
		}
	}
	
	private void ClearDisplay() {
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for(int j=0; j<distanceMatrix.GetLength(1); j++) {
				if( quadMatrix[i,j].GetComponent<BattlefieldQuad>().movementPenalty > 0 ) {
					// this quad is in range
					quadMatrix[i,j].renderer.material = defaultMat;
				}
			}
		}
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
			
			if( currValue > distance && range > distance + penalty ) {
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
	
	private Vector3[] GetWaypoints() {
		Vector3[] wps = new Vector3[path.Count+1];
		
		float x = 0;
		float y = transform.position.y;
		float z = 0;
		
		for( int i=0; i<path.Count; i++ ) {
			// get X and Z coordiante from quad
			x = quadMatrix[path[i][0], path[i][1]].transform.position.x;
			z = quadMatrix[path[i][0], path[i][1]].transform.position.z;
			
			wps[i+1] = new Vector3(x,y,z);
		}
		wps[0] = transform.position;
		return wps;
	}
}
