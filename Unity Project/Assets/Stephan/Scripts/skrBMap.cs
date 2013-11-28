using UnityEngine;
using System.Collections;

public class skrBMap : MonoBehaviour {
	// "B" is for behaviour; its part of the VIEW

	public GameObject blockPrefab;
	public int width;
	public int height;

	skrBUnit[] unitViews;
	skrBMapTile[] mapTileViews;

	Controller controller;

	// Use this for initialization
	void Start () {
		Init ();
	}

	void Init () 
	{
		unitViews = GameObject.FindObjectsOfType<skrBUnit>();
		controller = new Controller(this);
		controller.Initialized += HandleInitialized;
		controller.Init(width, height);

		GameObject.FindObjectOfType<skrInputTranslater>().Init(controller);
	}

	void HandleInitialized (object sender, InitializedEvent e)
	{
		InitMapTiles (e.maptiles);

		unitViews[0].unit.mapTile = mapTileViews[55].mapTile;
		controller.MoveUnit (unitViews [0].unit, mapTileViews [11].mapTile);
	}

	public void InitMapTiles(MapTile[,] mapTiles) 
	{
		Generate (mapTiles);
	}

	public void HandleUnitMoved (object sender, UnitMovedEvent e) 
	{
		skrBUnit unitToMove = GetBUnit (e.unit);
		Vector3 startPosition = GetBMapTile (e.source).transform.position;
		Vector3 endPosition = GetBMapTile (e.target).transform.position;

		unitToMove.MoveUnit (startPosition, endPosition);
	}

	skrBUnit GetBUnit (skrUnit unit)
	{
		foreach (skrBUnit view in unitViews) {
			if(view.unit == unit) {
				return view;
			}
		}
		return null;
	}

	skrBMapTile GetBMapTile (MapTile mapTile) {
		foreach (skrBMapTile view in mapTileViews) {
			if( view.mapTile == mapTile ) {
				return view;
			}
		}
		return null;
	}

	/**
	 * Instatiate blocks and position them in a grid with given number of columns and rows.
	 */ 
	public void Generate(MapTile[,] mapTiles) {
		mapTileViews = new skrBMapTile[mapTiles.Length];

		// Create rows of the grid
		for(int i = 0; i < mapTiles.GetLength(0); i++ ) {
			// Create columns of the grid
			for(int j = 0; j < mapTiles.GetLength(1); j++ ) {
				// Instatiate gameobject from prefab
				GameObject handle = (GameObject) Instantiate(blockPrefab);	
				// Give gameobject a decent name
				handle.name = blockPrefab.name + "[" + i + "," + j + "]";
				// Make gameobject a child of us
				handle.transform.parent = transform;
				// Move child to it's position on the grid
				handle.transform.localPosition = new Vector3( i, 0, j );

				int index = i * mapTiles.GetLength(1) + j;
				mapTileViews[index] = handle.GetComponent<skrBMapTile>();
				mapTileViews[index].mapTile = mapTiles[i,j];
			}
		}
	}
}
