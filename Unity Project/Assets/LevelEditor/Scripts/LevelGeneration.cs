using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelGeneration : MonoBehaviour {
	
	public GameObject blockPrefab;
	public int columns = 2;
	public int rows = 2;
	
	int[,] penaltyMatrix;
	GameObject[,] quadMatrix;
	
	void Start() {
		penaltyMatrix = new int[columns,rows];
		quadMatrix = new GameObject[columns,rows];
		
		for(int i=0; i<columns; i++ ) {
			for(int j=0; j<rows; j++ ) {
				quadMatrix[i,j] = GameObject.Find(blockPrefab.name + "[" + i + "," + j + "]");
				penaltyMatrix[i,j] = quadMatrix[i,j].GetComponent<BattlefieldQuad>().movementPenalty;
			}
		}
	}
	
	/**
	 * Instatiate blocks and position them in a grid with given number of columns and rows.
	 */ 
	public void Generate() {
		//progress = 0.0f;
		DoGeneration();	
	}
	
	public void DoGeneration() {
		// instanciate penalty matrix
		penaltyMatrix = new int[columns,rows];
		quadMatrix = new GameObject[columns, rows];
		
		// Create columns of the grid
		for(int i=0; i<columns; i++ ) {
			// Create rows of the grid
			for(int j=0; j<rows; j++ ) {
				// Instatiate gameobject from prefab
				GameObject handle = (GameObject) Instantiate(blockPrefab);	
				// Give gameobject a decent name
				handle.name = blockPrefab.name + "[" + i + "," + j + "]";
				// Make gameobject a child of us
				handle.transform.parent = transform;
				// Move child to it's position on the grid
				handle.transform.localPosition = new Vector3( i, 0, j );
				
				quadMatrix[i,j] = handle;
				
				// Set default value of penalty matrix
				penaltyMatrix[i,j] = 1;
			}
		}	
	}
	
	public void UpdatePenalty(int i, int j, int penalty) {
		if( penaltyMatrix == null ) {
			// instanciate penalty matrix
			penaltyMatrix = new int[columns,rows];
		}
		penaltyMatrix[i,j] = penalty;	
	}
	
	/**
	 * Remove all children elements, to clear generated level
	 */ 
	public void Reset() {
		for( int i=transform.childCount-1; i>=0; i-- ){
			if( Application.isEditor ) {
				GameObject.DestroyImmediate( transform.GetChild( i ).gameObject );	
			} else {
				GameObject.Destroy( transform.GetChild( i ).gameObject	);
			}
		}
	}

	/*public float Progress {
		get {
			return this.progress;
		}
	}*/

	public int[,] PenaltyMatrix {
		get {
			return this.penaltyMatrix;
		}
	}

	public GameObject[,] QuadMatrix {
		get {
			return this.quadMatrix;
		}
	}
}