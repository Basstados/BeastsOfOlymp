using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public GameObject blockPrefab;
	public int columns = 2;
	public int rows = 2;
	
	Quad[,] quadMatrix;
	//List<Monster> monsterList = new List<Monster>();
	
	void Start() {
		quadMatrix = new Quad[columns,rows];
		
		for(int i=0; i<columns; i++ ) {
			for(int j=0; j<rows; j++ ) {
				BattlefieldQuad currQuadMono = GameObject.Find(blockPrefab.name + "[" + i + "," + j + "]").GetComponent<BattlefieldQuad>();
				Vector2 pos = new Vector2( (int) currQuadMono.transform.position.x, (int) currQuadMono.transform.position.z );
				int pen = currQuadMono.movementPenalty;
				quadMatrix[i,j] = new Quad(pos, pen);
			}
		}
	}
	
	/**
	 * Instatiate blocks and position them in a grid with given number of columns and rows.
	 */ 
	public void Generate() {
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
			}
		}
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
	
	/**
	 * This will be send when the player has taped a quad on the field.
	 * 
	 * @param: exspect values of pos to be int values
	 */
	public void TapOnQuad( Vector2 pos ) {
		Quad currentQuad = quadMatrix[(int)pos.x,(int)pos.y];
		// do something with the taped quad
		Debug.Log( currentQuad );
	}

	public Quad[,] QuadMatrix {
		get {
			return this.quadMatrix;
		}
	}
}
