using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Algorithms;

public class Prototype_Map : MonoBehaviour {
	
	// parameters used to generate the map
	public GameObject blockPrefab;
	public int columns = 2;
	public int rows = 2;
	
	public Material reachableMat;
	public Material pathMat;
	public GameObject enemy;
	
	
	// data about everything that exists on the map
	Quad[,] quadMatrix;
	Dictionary<Vector2, Monster> monsterList;
	
	
	int[,] distanceMatrix;
	int range;
	Monster activeMonster;
	Vector3[] wps;
	TapMode currentTapMode = TapMode.PICK_MONSTER;
	
	enum TapMode {
		PICK_MONSTER,
		PICK_TARGET
	}
	
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
				quadMatrix[i,j] = new Quad(pos, pen, currQuadMono.gameObject);
			}
		}
		
		// find all monsters and add them to the dictionary
		monsterList = new Dictionary<Vector2, Monster>(); // start with a clear monster dictionary
		GameObject[] monsterGOs = GameObject.FindGameObjectsWithTag("Monster");
		foreach( GameObject go in monsterGOs ) {
			// use map position as key
			Vector2 pos = WorldToMapPosition( go.transform.position );
			Monster mon = go.GetComponent<Monster>();
			if( !mon ) {
				Debug.LogError("Monster has no 'Monster' MonoBehaviour attached!");	
			}
			
			// make sure the monster has a valid position
			if( 0 <= pos.x && pos.x < columns 
				&& 0<= pos.y && pos.y < rows ) {
				// monster pos is on the map
				
				// move monster the map position to make sure we don't get any glitches
				// because of flooring
				go.transform.position = transform.InverseTransformPoint( new Vector3( pos.x, 0f, pos.y ) );
				mon.CurrentPos = pos;
				// finally add monster to dictionary, using postion as key
				monsterList.Add(pos, mon);
				quadMatrix[ (int) pos.x, (int) pos.y ].penalty = -1;
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
		
		switch( currentTapMode ) {
			case TapMode.PICK_MONSTER:
				if( monsterList.ContainsKey( currentQuad.position ) ) {
					if( monsterList[currentQuad.position].controlable ) {
						// translate map position into screen position
						Vector3 worldPos = MapToWorldPosition( pos );
						Vector3 screenPos = Camera.main.WorldToScreenPoint( worldPos );
						screenPos.z = 0f;
						//combatMenu.OpenForMonster(screenPos, monsterList[currentQuad.position] );
					} else {
						combatMenu.SendMessage("Hide");
					}
				}
				break;
			case TapMode.PICK_TARGET:
				if( distanceMatrix[(int) pos.x, (int) pos.y] < range ) {
					// quad inside action range was clicked
					Debug.Log("Action target selected");
					activeMonster.PerformAction( pos );
					
				} else {
					// quad outside of action range was clicked
				
					// go back to combat menu
					ResetMapDisplay();
					Vector3 worldPos = MapToWorldPosition( activeMonster.CurrentPos );
					Vector3 screenPos = Camera.main.WorldToScreenPoint( worldPos );
					screenPos.z = 0f;
					combatMenu.OpenForMonster(screenPos, activeMonster );
				}
			break;
		}
		
	}
	
	public int[,] CalculateDistanceMatrix( Vector2 pos, int rng ) {
		// set distances to int.MaxValue as default
		InitDistanceMatrix();
		range = rng;
		if( monsterList[ pos ] ) {
			activeMonster = monsterList[ pos ];
		}
		
		// distance to current position is always 0
		distanceMatrix[(int) pos.x,(int) pos.y] = 0;
		// start recursiv distance calulation
		CheckNeighbourQuad((int) pos.x, (int) pos.y, 0);
		
		return distanceMatrix;
	}

	public int[,] CalculateDistanceMatrixAttackMode( Vector2 pos, int rng) {
		// set distances to int.MaxValue as default
		InitDistanceMatrix();
		range = rng;
		if( monsterList[ pos ] ) {
			activeMonster = monsterList[ pos ];
		}
		
		// distance to current position is always 0
		distanceMatrix[(int) pos.x,(int) pos.y] = 0;
		// start recursiv distance calulation
		CheckNeighbourQuadAttackMode((int) pos.x, (int) pos.y, 0);
		
		return distanceMatrix;
	}
	
	public void DisplayDistanceMatrix( int[,] dstMatrix, int rng ) {
		distanceMatrix = dstMatrix;
		range = rng;
		currentTapMode = TapMode.PICK_TARGET;
		
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for(int j=0; j<distanceMatrix.GetLength(1); j++) {
				if( distanceMatrix[i,j] < range ) {
					// this quad is in range
					quadMatrix[i,j].ChangeMaterial( reachableMat );
				}
			}
		}
	}
	
	public void ResetMapDisplay() {
		for(int i=0; i<quadMatrix.GetLength(0); i++) {
			for(int j=0; j<quadMatrix.GetLength(1); j++) {
				quadMatrix[i,j].ResetMaterial();
			}
		}
	}
	
	/**
	 * Initalize a new distance matrix with all values int.MaxValue
	 */ 
	private void InitDistanceMatrix() {
		distanceMatrix = new int[quadMatrix.GetLength(0), quadMatrix.GetLength(1)];
		// set all values to int.MaxValue (for unreachable) as default
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for( int j=0; j<distanceMatrix.GetLength(1); j++) {
				distanceMatrix[i,j] = int.MaxValue;
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
			int penalty = quadMatrix[neighbour[k][0], neighbour[k][1]].penalty;
			
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

	/**
	 * [i,j] are the indices of the previous field
	 * ignore penelty of field
	 */ 
	private void CheckNeighbourQuadAttackMode(int i, int j, int distance) {
		
		// neighbour quad indicies
		List<int[]> neighbour = GetNeighbourList(i,j);
		
		// check all neighbours if there are in range and continue recursion
		for( int k=0; k<neighbour.Count; k++) {
			int currValue = distanceMatrix[neighbour[k][0], neighbour[k][1]];
			int penalty = 1;
			
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
	
	public List<int[]> CalculatePath(int[] start, int[] goal ) {
		if( start[0] == goal[0] && start[1] == goal[1] ) {
			List<int[]> result = new List<int[]>();
			result.Add( start );
			return result;
		}

		Point myPos = new Point(start[0], start[1]);
		Point targetPos = new Point(goal[0], goal[1]);

		byte[,] grid = new byte[quadMatrix.GetLength(0),quadMatrix.GetLength(1)];
		for( int i=0; i< quadMatrix.GetLength(0); i++ ) {
			for( int j=0; j<quadMatrix.GetLength(1); j++ ) {
				grid[i,j] = (quadMatrix[i,j].penalty > 0) ? (byte) 1 : (byte) 0;
				// grid value for our current pos should be 1, otherwise we get into trouble
				if( (i == myPos.x && j == myPos.y) || (i == targetPos.x && j == targetPos.y ) ){
					grid[i,j] = 1;
				}
			}
		}
		
		PathFinder pf = new PathFinder( grid );
		pf.Diagonals = false;
		
		Debug.Log( pf );
		Debug.Log( myPos.ToString() );
		List<PathFinderNode> path = pf.FindPath( myPos, targetPos );
		Debug.Log( path );
		
		List<int[]> intPath = new List<int[]>();
		string str = "";
		foreach( PathFinderNode p in path ) {
			str += "[" + p.X + "," + p.Y + "]";
			intPath.Add( new int[2]{ p.X, p.Y } );
		}
		Debug.Log( str );

		quadMatrix[goal[0],goal[1]].ChangeMaterial( pathMat );

		return intPath;
	}
	
	public Vector3[] GetWaypoints(List<int[]> path) {
		Vector3[] wps = new Vector3[path.Count];
		
		float x = 0;
		float y = transform.position.y;
		float z = 0;
		
		for( int i=0; i<path.Count; i++ ) {
			// get X and Z coordiante from quad
			x = quadMatrix[path[i][0], path[i][1]].position.x;
			z = quadMatrix[path[i][0], path[i][1]].position.y;
			
			wps[i] = new Vector3(x,y,z);
		}
		//wps[0] = MapToWorldPosition( activeMonster.CurrentPos );
		return wps;
	}
	
	public void MoveMonster( Monster mon, Vector2 oldPos, Vector2 newPos) {
		if( mon == monsterList[oldPos] ) {
			monsterList.Remove( oldPos );
			monsterList.Add( newPos, mon );
			quadMatrix[ (int) oldPos.x, (int) oldPos.y ].penalty = 1;
			quadMatrix[ (int) newPos.x, (int) newPos.y ].penalty = -1;
			currentTapMode = TapMode.PICK_MONSTER;
		}
	}

	public void RemoveMonster( Vector2 pos ) {
		// respawn on a random position
		Vector2 newPos = new Vector2( (int) Random.Range(0, quadMatrix.GetLength(0)-1), (int) Random.Range(0,quadMatrix.GetLength(1)-1));
		while( monsterList.ContainsKey( newPos ) ) {
			newPos = new Vector2( (int) Random.Range(0, quadMatrix.GetLength(0)-1), (int) Random.Range(0,quadMatrix.GetLength(1)-1));
		}
		Monster mon = monsterList[pos];
		mon.hp = 10;
		mon.CurrentPos = newPos;
		mon.transform.position = new Vector3( newPos.x, 0, newPos.y );
		MoveMonster( mon, pos, newPos );
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
	
	public Monster GetMonsterAt( Vector2 pos ) {
		if( monsterList.ContainsKey( pos ) ) {
			return monsterList[pos];
		} else {
			return null;
		}
	}

	public List<int[]> GetShortestPath( int[] start, int[] destination ) {
		// set range to MaxValue to get distance matrix for full map (dirty, slow but easy for now)
		range = int.MaxValue;
		distanceMatrix = CalculateDistanceMatrix( new Vector2(start[0], start[1]), range );
		List<int[]> path = CalculatePath( start, destination );
		int cost = distanceMatrix[destination[0], destination[1]];

		return path;
	}
	
	public void ActionFinished() {
		currentTapMode = TapMode.PICK_MONSTER;	
	}
}
