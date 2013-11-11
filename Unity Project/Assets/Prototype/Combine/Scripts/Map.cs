using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	
	// parameters used to generate the map
	public GameObject blockPrefab;
	public int columns = 2;
	public int rows = 2;
	
	
	// data about everything that exists on the map
	Quad[,] quadMatrix;
	Dictionary<Vector2, Monster> monsterList;
	
	
	// ui elements
	public CombatMenu combatMenu;
	
	void Start() {
		quadMatrix = new Quad[columns,rows];
		
		// initialize quadmatrix by getting the Battlefield quads from the scene
		for(int i=0; i<columns; i++ ) {
			for(int j=0; j<rows; j++ ) {
				BattlefieldQuad currQuadMono = GameObject.Find(blockPrefab.name + "[" + i + "," + j + "]").GetComponent<BattlefieldQuad>();
				Vector2 pos = new Vector2( (int) currQuadMono.transform.position.x, (int) currQuadMono.transform.position.z );
				int pen = currQuadMono.movementPenalty;
				quadMatrix[i,j] = new Quad(pos, pen);
			}
		}
		
		// find all monsters and add them to the dictionary
		monsterList = new Dictionary<Vector2, Monster>(); // start with a clear monster dictionary
		GameObject[] monsterGOs = GameObject.FindGameObjectsWithTag("Monster");
		foreach( GameObject go in monsterGOs ) {
			// use map position as key
			Vector2 pos = WorldToMapPosition( go.transform.position );
			Monster mon = go.GetComponent<Monster>();
			
			// make sure the monster has a valid position
			if( 0 <= pos.x && pos.x < columns 
				&& 0<= pos.y && pos.y < rows ) {
				// monster pos is on the map
				
				// move monster the map position to make sure we don't get any glitches
				// because of flooring
				go.transform.position = transform.InverseTransformPoint( new Vector3( pos.x, 0f, pos.y ) );
				// finally add monster to dictionary, using postion as key
				monsterList.Add(pos, mon);
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

		if( monsterList.ContainsKey( currentQuad.position ) ) {
			// translate map position into screen position
			Vector3 worldPos = MapToWorldPosition( pos );
			Vector3 screenPos = Camera.main.WorldToScreenPoint( worldPos );
			screenPos.z = 0f;
			combatMenu.SendMessage("MoveTo", screenPos );
		} else {
			combatMenu.SendMessage("Hide");
		}
	}
	
	/**
	 * Translate any 3D world position into a discrete 2D map position
	 * by taking X- and Z-coordiante and floor them.
	 */
	public Vector2 WorldToMapPosition( Vector3 worldPos ) {
		Vector3 temp = transform.TransformPoint( worldPos );
		Vector2 mapPos = Vector2.zero;
		mapPos.x = Mathf.Floor( temp.x );
		mapPos.y = Mathf.Floor( temp.z );
		
		return mapPos;
	}
	
	/**
	 * Translate any 2D map postion into a 3D word position.
	 */ 
	public Vector3 MapToWorldPosition( Vector2 mapPos ) {
		Vector3 temp = new Vector3( mapPos.x, 0f, mapPos.y );
		Vector3 worldPos = transform.InverseTransformPoint( temp );
		
		return worldPos;
	}

	public Quad[,] QuadMatrix {
		get {
			return this.quadMatrix;
		}
	}
}
