using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class skrBMap : MonoBehaviour {
	// "B" is for behaviour; its part of the VIEW

	public GameObject unitPrefab;
	public GameObject blockPrefab;
	public int width;
	public int height;

	List<skrBUnit> unitViews;
	skrBMapTile[] mapTileViews;

	skrController controller;

	// Use this for initialization
	void Start () {
		Init ();
	}

	void Init () 
	{
		controller = new skrController(this);
		controller.Initialized += HandleInitialized;
		controller.Init(width, height);

		GameObject.FindObjectOfType<skrInputTranslater>().Init(controller);
	}

	void HandleInitialized (object sender, InitializedEvent e)
	{
		InitMapTiles (e.maptiles);
	}

	public void InitMapTiles(skrMapTile[,] mapTiles) 
	{
		Generate (mapTiles);
	}

	public void HandleUnitMoved (object sender, skrUnitMovedEvent e) 
	{
		skrBUnit unitToMove = GetBUnit (e.unit);
		Vector3 startPosition = GetBMapTile (e.source).transform.position;
		Vector3 endPosition = GetBMapTile (e.target).transform.position;

		unitToMove.MoveUnit (startPosition, endPosition);
	}

	public void HandleUnitSpawned (object sender, skrUnitSpawnedEvent e)
	{
		Vector3 spawnPosition = GetBMapTile (e.unit.mapTile).transform.position;
		// instatiate unit prefab
		GameObject handle = Instantiate (unitPrefab) as GameObject;
		handle.transform.position = spawnPosition;
		handle.GetComponent<skrBUnit>().unit = e.unit;

		// add new spawned unit to unitView list
		if (unitViews == null)
			unitViews = new List<skrBUnit> ();

		unitViews.Add(handle.GetComponent<skrBUnit>());
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

	skrBMapTile GetBMapTile (skrMapTile mapTile) 
	{
		// use Linq like SQL statements to reduce/order list
		return mapTileViews.Where (t => t.mapTile == mapTile).FirstOrDefault();
	}

	/**
	 * Instatiate blocks and position them in a grid with given number of columns and rows.
	 */ 
	public void Generate(skrMapTile[,] mapTiles) {
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
