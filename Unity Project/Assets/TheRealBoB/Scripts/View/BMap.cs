using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BMap : MonoBehaviour {

	[SerializeField] GameObject bMapTilePrefab;
	[SerializeField] int gridSizeX = 10;
	[SerializeField] int gridSizeY = 10;
	[SerializeField] BMapTile[] bMapTiles;

	[SerializeField] GameObject obstaclePrefab;
	[SerializeField] int obstacleCount = 20;
	[SerializeField] GameObject[] obstacles;

	/// <summary>
	/// Direct access to the bMapTiles of this bMap.
	/// </summary>
	/// <param name="i">The x index.</param>
	/// <param name="j">The y index.</param>
	public BMapTile this[int i,int j]
	{
		get { return bMapTiles[i + j * gridSizeX]; }
		set { bMapTiles[i + j * gridSizeX] = value; }
	}

	public int lengthX { get{return gridSizeX; }}
	public int lengthY { get{return gridSizeY; }}

	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Remove old map and instantiate new mapTiles.
	/// </summary>
	public void InstantiateMap()
	{
		if(!Application.isEditor) return;
		// destroy old map
//		if(bMapTiles != null)
//			for (int i = 0; i < bMapTiles.Length; i++) {
//				for (int j = 0; j < bMapTiles[i].Length; j++) {
//				if(bMapTiles[i + j * gridSizeX].gameObject != null)
//						DestroyImmediate(bMapTiles[i][j].gameObject);
//				}
//			}
		List<GameObject> children = new List<GameObject>();
		for (int i = 0; i < transform.childCount; i++) {
			children.Add(transform.GetChild(i).gameObject);
		}
		foreach(GameObject go in children)
			DestroyImmediate(go);

		bMapTiles = new BMapTile[gridSizeX * gridSizeY];
		for (int i = 0; i < gridSizeX; i++) {
			for (int j = 0; j < gridSizeY; j++) {
				Vector3 pos = this.transform.TransformPoint(new Vector3(i, 0, j));
				GameObject go = Instantiate(bMapTilePrefab, pos, bMapTilePrefab.transform.rotation) as GameObject;
				// parenting
				go.transform.parent = this.transform;
				// proper nameing
				go.name = bMapTilePrefab.name + "[" + i + "," + j + "]";
				// set references
				bMapTiles[i + j * gridSizeX] = go.GetComponent<BMapTile>();
				bMapTiles[i + j * gridSizeX].mapTile = new MapTile(i,j);
			}
		}
	}

	/// <summary>
	/// Delete old obstacles and place new obstalces randomly on the map.
	/// Doesn't place obstalce on fields taken by units.
	/// </summary>
	public void SpawnObstacles() 
	{
		// clear old obstacles
		foreach(GameObject ob in obstacles) {
			// reset penalty
			int x = Mathf.FloorToInt(ob.transform.localPosition.x);
			int y = Mathf.FloorToInt(ob.transform.localPosition.z);
			bMapTiles[x + y *gridSizeX].mapTile.Penalty = 1;
			// destroy gameobject
			if(Application.isEditor) {
				DestroyImmediate(ob);
			} else {
				Destroy(ob);
			}
		}

		// create list of taken fields
		List<Vector> takenFields = new List<Vector>();
		// add units in scene to taken fields
		BUnit[] startBUnits = GameObject.FindObjectsOfType<BUnit>();
		foreach(BUnit bUnit in startBUnits) {
			takenFields.Add(new Vector(Mathf.FloorToInt(bUnit.transform.position.x),
			                           Mathf.FloorToInt(bUnit.transform.position.z)));
		}

		// instatiate new obstacles at random (free) postions
		obstacles = new GameObject[obstacleCount];
		for (int i = 0; i < obstacleCount; i++) {
			// find free field
			Vector vec;
			int safetyCount = 0;
			do {
				vec = new Vector(Random.Range(0,gridSizeX),Random.Range(0,gridSizeY));
				safetyCount++; // prevent infinit loops
				if(safetyCount > 500) {
					Debug.LogError("No free field found!");
					return;
				}
			} while(takenFields.Contains(vec));
			// spawn obstacle at field
			Vector3 pos = new Vector3(vec.x, 0, vec.y);
			GameObject handle = Instantiate(obstaclePrefab, pos, obstaclePrefab.transform.rotation) as GameObject;
			handle.transform.parent = this.transform;
			handle.name = "Obstacle " + i;

			obstacles[i] = handle;
			bMapTiles[vec.x + vec.y * gridSizeX].mapTile.Penalty = 0;
		}
	}

	/// <summary>
	/// Update mapTile penaltys respecting obstacles
	/// </summary>
	public void UpdateMap() 
	{
		// reset mapTile penlatys
		foreach(BMapTile bMapTile in bMapTiles) {
			bMapTile.mapTile.Penalty = 1; // penalty = 1 is the default value
		}

		// apply penlaty for mapTiles taken by obstacles
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject ob in obstacles) {
			int x = Mathf.FloorToInt(ob.transform.localPosition.x);
			int y = Mathf.FloorToInt(ob.transform.localPosition.z); // world z is 2nd coordiante on the grid (y)
			bMapTiles[x + y *gridSizeX].mapTile.Penalty = 0; // pentaly = 0  <=> maptile is taken
		}
	}
}
