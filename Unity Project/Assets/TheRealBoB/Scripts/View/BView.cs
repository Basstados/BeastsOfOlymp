using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BView : MonoBehaviour
{
	public GameObject bMapTilePrefab;
	public GameObject bUnitPrefab;
	public GameObject bCombatMenuPrefab;

	// context references
	BMapTile[][] bMapTiles;
	List<BUnit> bUnits = new List<BUnit>();
	BUnit activeBUnit;
	BCombatMenu bCombatMenu;
	BInputManager bInputManager;
	BCameraMover bCameraMover;

	public Controller controller;

	void Awake() {
		Init();
	}

	void Init() {
		// register event
		EventProxyManager.RegisterForEvent(EventName.Initialized, HandleInitialized);
		EventProxyManager.RegisterForEvent(EventName.UnitSpawned, HandleUnitSpawned);
		EventProxyManager.RegisterForEvent(EventName.UnitActivated, HandleUnitActivated);
		EventProxyManager.RegisterForEvent(EventName.PlayersTurnStarted, HandlePlayerTurnStarted);
		EventProxyManager.RegisterForEvent(EventName.MoveActionSelected, HandleMoveSelected);
		EventProxyManager.RegisterForEvent(EventName.BMapTileTapped, HandleBMapTileTapped);
		EventProxyManager.RegisterForEvent(EventName.UnitMoved, HandleUnitMoved);
		// find scene references
		bCombatMenu = GameObject.FindObjectOfType<BCombatMenu>();
		bInputManager = GameObject.FindObjectOfType<BInputManager>();
		bCameraMover = GameObject.FindObjectOfType<BCameraMover>();
		// start the game
		controller = new Controller();
	}

	#region event handler
	void HandleInitialized(object sender, EventArgs args)
	{
		if(sender.GetType () == typeof(Model)) {
			InstatiateMap((args as MapInitializedEvent).mapTiles);
		}
	}

	void HandleUnitSpawned(object sender, EventArgs args)
	{
		UnitSpawnedEvent e = args as UnitSpawnedEvent;

		SpawnBUnit(e.unit);
	}

	void HandleUnitActivated(object sender, EventArgs args)
	{
		UnitActivatedEvent e = args as UnitActivatedEvent;

		activeBUnit = GetBUnit(e.unit);
		bCameraMover.Focus(activeBUnit.gameObject);
	}

	void HandlePlayerTurnStarted(object sender, EventArgs args)
	{
		UnitActivatedEvent e = args as UnitActivatedEvent;
		BUnit bUnit = GetBUnit(e.unit);
		bUnit.PopupCombatMenu();
	}

	void HandleMoveSelected (object sender, EventArgs args)
	{
		MoveActionSelectedEvent e = args as MoveActionSelectedEvent;
		e.bUnit.DisplayMovementRange();
	}

	void HandleBMapTileTapped (object sender, EventArgs args)
	{
		BMapTileTappedEvent e = args as BMapTileTappedEvent;
		if(e.bMapTile.colorState == BMapTile.ColorState.INRANGE)
			activeBUnit.SetTarget(e.bMapTile);
	}

	void HandleUnitMoved (object sender, EventArgs args)
	{
		UnitMovedEvent e = args  as UnitMovedEvent;
		// convert MapTile path to BMapTile path
		BMapTile[] path = new BMapTile[e.path.Length];
		for (int i = 0; i < e.path.Length; i++) {
			path[i] = GetBMapTile(e.path[i]);
		}
		// send movement path to BUnit
		GetBUnit(e.unit).MoveAlongPath(path);

		// clean up map color
		foreach(BMapTile[] column in bMapTiles)
			foreach(BMapTile tile in column)
				tile.ChangeColorState(BMapTile.ColorState.DEFAULT);
	}

	#endregion

	/// <summary>
	/// Instatiates the map grid.
	/// </summary>
	/// <param name="mapTiles">Reference to the mapTile instances</param>
	void InstatiateMap(MapTile[][] mapTiles) 
	{
		bMapTiles = new BMapTile[mapTiles.Length][];

		for (int i = 0; i < bMapTiles.Length; i++) {
			bMapTiles[i] = new BMapTile[mapTiles[i].Length];

			for (int j = 0; j < mapTiles[i].Length; j++) {
				GameObject go = Instantiate(bMapTilePrefab) as GameObject;
				// parenting
				go.transform.parent = this.transform;
				// proper nameing
				go.name = bMapTilePrefab.name + "[" + i + "," + j + "]";
				// positioning
				go.transform.localPosition = new Vector3(i, 0, j);
				// set references
				bMapTiles[i][j] = go.GetComponent<BMapTile>();
				bMapTiles[i][j].mapTile = mapTiles[i][j];
			}
		}
	}

	/// <summary>
	/// Instatiate and initialize BUnit which is the view representation of an Unit
	/// </summary>
	/// <param name="unit">The Unit we create an representation for</param>
	void SpawnBUnit(Unit unit) {
		GameObject go = Instantiate(bUnitPrefab) as GameObject;
		// positioning
		go.transform.position = GetBMapTile(unit.mapTile).transform.position;
		// set references 
		BUnit bUnit = go.GetComponent<BUnit>();
		bUnit.Init(this, unit, bCombatMenu);
		// add to list
		bUnits.Add(bUnit);
	}

	/// <summary>
	/// Get the distance matrix from the controller and use it to display a range.
	/// </summary>
	/// <param name="movementRange">Maximum range for the distance matrix.</param>
	public void DisplayRange(BUnit bUnit, int range)
	{
		// update InputPhase
		bInputManager.phase = BInputManager.InputPhase.PICKTARGET;

		// get distance matrix
		Point position = new Point(bUnit.unit.mapTile.x, bUnit.unit.mapTile.y);
		byte[][] distMatrix = controller.GetDistanceMatrix(position, range);
		// change color of all BMapTiles in range
		for (int i = 0; i < distMatrix.Length; i++) {
			for (int j = 0; j < distMatrix[i].Length; j++) {
				if(range >= distMatrix[i][j] && 0 < distMatrix[i][j]) {
					bMapTiles[i][j].ChangeColorState(BMapTile.ColorState.INRANGE);
				}
			}
		}
	}

	/// <summary>
	/// Gets the BMapTile representation for a MapTile
	/// </summary>
	/// <returns>The BMapTile.</returns>
	/// <param name="mapTile">MapTile</param>
	BMapTile GetBMapTile(MapTile mapTile) 
	{
		return bMapTiles[mapTile.x][mapTile.y];
	}

	/// <summary>
	/// Gets the BUnit representation for an Unit
	/// </summary>
	/// <returns>The BUnit.</returns>
	/// <param name="unit">Unit</param>
	BUnit GetBUnit(Unit unit)
	{
		return bUnits.Where(t => t.unit == unit).FirstOrDefault();
	}
}

