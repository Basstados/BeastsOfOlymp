using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BView : MonoBehaviour
{
	public GameObject bMapTilePrefab;
	public GameObject bUnitPrefab;
	public GameObject bCombatMenuPrefab;

	BMapTile[][] bMapTiles;
	List<BUnit> bUnits = new List<BUnit>();
	
	BCombatMenu bCombatMenu;

	void Awake() {
		Init();
	}

	void Init() {
		// register event
		EventProxyManager.RegisterForEvent(EventName.Initialized, HandleInitialized);
		EventProxyManager.RegisterForEvent(EventName.UnitSpawned, HandleUnitSpawned);
		EventProxyManager.RegisterForEvent(EventName.PlayersTurnStarted, HandlePlayerTurnStarted);
		// find scene references
		bCombatMenu = GameObject.FindObjectOfType<BCombatMenu>();

		// start the game
		Controller controller = new Controller();
	}

	void HandleInitialized(object sender, EventArgs args)
	{
		if(sender.GetType () == typeof(Model)) {
			InstatiateMap((args as MapInitializedEvent).mapTiles);
		}
	}

	void HandleUnitSpawned(object sender, EventArgs args)
	{
		UnitSpawnedEvent e = args as UnitSpawnedEvent;

		GameObject go = Instantiate(bUnitPrefab) as GameObject;
		// positioning
		go.transform.position = GetBMapTile(e.unit.mapTile).transform.position;
		// set references 
		BUnit bUnit = go.GetComponent<BUnit>();
		bUnit.Init(e.unit, bCombatMenu);
		// add to list
		bUnits.Add(bUnit);
	}

	void HandlePlayerTurnStarted(object sender, EventArgs args)
	{
		UnitActivatedEvent e = args as UnitActivatedEvent;
		BUnit bUnit = GetBUnit(e.unit);
		bUnit.PopupCombatMenu();
	}

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

	BMapTile GetBMapTile(MapTile mapTile) 
	{
		return bMapTiles[mapTile.x][mapTile.y];
	}

	BUnit GetBUnit(Unit unit)
	{
		return bUnits.Where(t => t.unit == unit).FirstOrDefault();
	}
}

