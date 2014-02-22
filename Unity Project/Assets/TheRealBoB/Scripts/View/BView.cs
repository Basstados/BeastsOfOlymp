using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BView : MonoBehaviour
{
	public GameObject bMapTilePrefab;
	public GameObject bUnitPrefab;
	public GameObject bCombatMenuPrefab;
	public GameObject bActiveMarkerPrefab;
	public GameObject bMoveMarkerPrefab;

	public BIniativeList bInitativeList;
	public BCombatLog bCombatLog;

	// context references
	BMapTile[][] bMapTiles;
	List<BUnit> bUnits = new List<BUnit>();
	BUnit activeBUnit;
	BCombatMenu bCombatMenu;
	BInputManager bInputManager;
	BCameraMover bCameraMover;

	GameObject unitMarker;
	GameObject moveMarker;

	Queue<EventProxyArgs> eventQueue = new Queue<EventProxyArgs>();
	bool performingEvent = false;

	public Controller controller;

	void Awake() {
		Init();
	}

	void Init() {
		performingEvent = true;

		// register event
		EventProxyManager.RegisterForEvent(EventName.Initialized, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.UnitSpawned, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.UnitActivated, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.TurnStarted, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.BMapTileTapped, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.BUnitTapped, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.UnitMoved, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.UnitAttacked, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.UnitDied, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.Gameover, HandleEvent);
		EventProxyManager.RegisterForEvent(EventName.EventDone, HandleEventDone);
		EventProxyManager.RegisterForEvent(EventName.DebugLog, HandleDebugLog);
		EventProxyManager.RegisterForEvent(EventName.CombatLogInitialized, HandleCombatLogInitialized);
		// find scene references
		bCombatMenu = GameObject.FindObjectOfType<BCombatMenu>();
		bInputManager = GameObject.FindObjectOfType<BInputManager>();
		bCameraMover = GameObject.FindObjectOfType<BCameraMover>();

		// instatiate marker
		unitMarker = (GameObject) Instantiate(bActiveMarkerPrefab);
		moveMarker = (GameObject) Instantiate(bMoveMarkerPrefab);

		// start the game
		controller = new Controller(this);

		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	#region event handler
	void HandleEvent(object sender, EventProxyArgs args)
	{
		if(args.name == EventName.TurnStarted) {
			TurnStartedEvent e = (TurnStartedEvent) args;
			Debug.Log("Round: " + e.round + " Turn: " + e.turn);
		}
		eventQueue.Enqueue(args);
		if(!performingEvent) 
			EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleEventDone(object sender, EventProxyArgs args)
	{
		// continue with next event from queue
		if(eventQueue.Count > 0) {
			EventProxyArgs eventArgs = eventQueue.Dequeue();
			Debug.Log(eventArgs.name);
			performingEvent = true;
			// call handler for the next event in the queue
			switch(eventArgs.name) {
			case EventName.Initialized:
				HandleInitialized(sender , eventArgs);
				break;
			case EventName.UnitSpawned:
				HandleUnitSpawned(sender, eventArgs);
				break;
			case EventName.RoundSetup:
				HandleRoundSetup(sender, eventArgs);
				break;
			case EventName.UnitActivated:
				HandleUnitActivated(sender, eventArgs);
				break;
			case EventName.TurnStarted:
				HandleTurnStarted(sender, eventArgs);
				break;
			case EventName.BMapTileTapped:
				HandleBMapTileTapped(sender, eventArgs);
				break;
			case EventName.BUnitTapped:
				HandleBUnitTapped(sender, eventArgs);
				break;
			case EventName.UnitMoved:
				HandleUnitMoved(sender, eventArgs);
				break;
			case EventName.UnitAttacked:
				HandleUnitAttacked(sender, eventArgs);
				break;
			case EventName.UnitDied:
				HandleUnitDied(sender, eventArgs);
				break;
			case EventName.Gameover:
				HandleGameover(sender, eventArgs);
				break;
			}
		} else {
			// we are done with event queue
			performingEvent = false;
		}
	}

	void HandleInitialized(object sender, EventArgs args)
	{
		InstatiateMap((args as MapInitializedEvent).mapTiles);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleCombatLogInitialized (object sender, EventProxyArgs args)
	{
		bCombatLog.Init((args as CombatLogInitializedEvent).combatLog);
	}

	void HandleUnitSpawned(object sender, EventArgs args)
	{
		UnitSpawnedEvent e = args as UnitSpawnedEvent;

		SpawnBUnit(e.unit);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleRoundSetup (object sender, EventProxyArgs args)
	{
		RoundSetupEvent e = args as RoundSetupEvent;

		bInitativeList.UpdateList(e.sortedList);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleUnitActivated(object sender, EventArgs args)
	{
		UnitActivatedEvent e = args as UnitActivatedEvent;

		activeBUnit = GetBUnit(e.unit);
		// place marker for active unit
		unitMarker.transform.parent = activeBUnit.transform;
		unitMarker.transform.localPosition = new Vector3(0,0.01f,0);

		bInitativeList.ActivateIcon(e.unit);
		bCameraMover.Focus(activeBUnit.gameObject.transform.position);
		activeBUnit.Activate();
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleTurnStarted(object sender, EventArgs args)
	{
		TurnStartedEvent e = args as TurnStartedEvent;
		BUnit bUnit = GetBUnit(e.unit);
		bUnit.PopupCombatMenu();
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleBMapTileTapped(object sender, EventArgs args)
	{
		// forward maptile to active unit 
		BMapTileTappedEvent e = args as BMapTileTappedEvent;
		if(e.bMapTile.Clickable) {
			activeBUnit.SetMoveTarget(e.bMapTile);
			activeBUnit.SetAttackTarget(e.bMapTile);
		}
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleBUnitTapped(object sender, EventArgs args)
	{
		// use tapped unit as attack target for active unit
		// if active unit is not ready to attack, nothing will happen
//		BUnitTappedEvent e = args as BUnitTappedEvent;
//		if(GetBMapTile(e.bUnit.unit.mapTile).Clickable)
//			activeBUnit.SetAttackTarget(e.bUnit);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleUnitMoved (object sender, EventArgs args)
	{
		UnitMovedEvent e = args  as UnitMovedEvent;
		// convert MapTile path to BMapTile path
		BMapTile[] path = new BMapTile[e.path.Length];
		for (int i = 0; i < e.path.Length; i++) {
			path[i] = GetBMapTile(e.path[i]);
		}
		// send movement path to BUnit for animation
		GetBUnit(e.unit).MoveAlongPath(path);
		bCameraMover.Focus(path.Last().transform.position);
		CleanMap();
	}

	void HandleUnitAttacked (object sender, EventArgs args)
	{
		UnitAttackedEvent e = args as UnitAttackedEvent;
		BUnit[] bUnits = new BUnit[e.targets.Count];
		for (int i = 0; i < e.targets.Count; i++) {
			bUnits[i] = GetBUnit(e.targets[i]);
		}

		GetBUnit(e.source).PlayAttack(bUnits, e.attack, e.efficieny, e.damage);
		CleanMap();
	}

	void HandleUnitDied (object sender, EventArgs args)
	{
		UnitDiedEvent e = args as UnitDiedEvent;
		BUnit bUnit = GetBUnit(e.unit);
		bCameraMover.Focus(bUnit.transform.position);
		bUnit.Died();
		
	}


	void HandleGameover (object sender, EventArgs args)
	{
		GameoverEvent e = args as GameoverEvent;
		string text = "";
		if(e.playerDefeated && e.aiDefeated)
			text = "Draw";
		else if(e.aiDefeated)
			text = "Victory";
		else if(e.playerDefeated)
			text = "Defeated";

		bCombatMenu.DisplayGameover(text);
		EventProxyManager.FireEvent(this, new EventDoneEvent());
	}

	void HandleDebugLog (object sender, EventProxyArgs args)
	{
		DebugLogEvent e = args as DebugLogEvent;

		Debug.Log(e.debugLogString);
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
		Debug.Log("Spawning: Character/"+unit.Name);
		GameObject prefab = Resources.Load<GameObject>("Character/"+unit.Name);
		GameObject go = (GameObject) Instantiate(prefab);
		// positioning
		go.transform.position = GetBMapTile(unit.mapTile).transform.position;
		// set references 
		BUnit bUnit = go.GetComponent<BUnit>();
		bUnit.Init(this, unit, bCombatMenu);
		// add to list
		bUnits.Add(bUnit);
	}

	public void DisplayArea(BMapTile bMaptile, Vector[] area)
	{
		int x = 0;
		int y = 0;

		foreach(Vector pt in area) {
			x = bMaptile.mapTile.x + pt.x;
			y = bMaptile.mapTile.y + pt.y;
			if(x >= 0 && x < bMapTiles.Length && y >= 0 && y < bMapTiles[0].Length)
				bMapTiles[x][y].ChangeColorState(BMapTile.ColorState.ATTACKAREA);
		}
	}

	/// <summary>
	/// Get the distance matrix from the controller and use it to display a range.
	/// </summary>
	/// <param name="movementRange">Maximum range for the distance matrix.</param>
	public void DisplayRange(BUnit bUnit, int range, int mode)
	{
		// update InputPhase
		bInputManager.phase = BInputManager.InputPhase.PICKTARGET;

		bool ignoreUnits = false;
		if(bUnit.CurrentAction == BUnit.Action.ATTACK)
			ignoreUnits = true;

		Debug.Log(bInputManager.phase);
		
		// get distance matrix
		Vector position = new Vector(bUnit.unit.mapTile.x, bUnit.unit.mapTile.y);
		byte[][] distMatrix = controller.GetDistanceMatrix(position, range, ignoreUnits);
		// change color of all BMapTiles in range
		for (int i = 0; i < distMatrix.Length; i++) {
			for (int j = 0; j < distMatrix[i].Length; j++) {
				if(range >= distMatrix[i][j]) {
					HighlightBMapTile(bMapTiles[i][j], mode);
				}
			}
		}
	}

	void HighlightBMapTile(BMapTile bMapTile, int mode)
	{
		switch(mode) 
		{
		case DisplayRangeMode.TEAM_0_CLICKABLE:
			// mark BMapTile as clickable if there is a unit of team 0 on top
			// use inRange material else
			if(bMapTile.mapTile.unit != null) {
				if(bMapTile.mapTile.unit.team == Unit.Team.AI) 
					bMapTile.ChangeColorState(BMapTile.ColorState.CLICKABLE);
			} else {
				bMapTile.ChangeColorState(BMapTile.ColorState.INRANGE);
			}
			break;
		case DisplayRangeMode.TEAM_1_CLICKABLE:
			// mark BMapTile as clickable if there is a unit of team 1 on top
			// use inRange material else
			if(bMapTile.mapTile.unit != null) {
				if(bMapTile.mapTile.unit.team == Unit.Team.PLAYER)
					bMapTile.ChangeColorState(BMapTile.ColorState.CLICKABLE);
			} else {
				bMapTile.ChangeColorState(BMapTile.ColorState.INRANGE);
			}
			break;
		case DisplayRangeMode.ALL_CLICKABLE:
			// mark the BMapTile alweays as clickable
			bMapTile.ChangeColorState(BMapTile.ColorState.CLICKABLE);
			break;
		}
	}

	/// <summary>
	/// Visually highlight the path from a given unit to the target.
	/// Is using the pathfinder algorithims of the Controller class.
	/// </summary>
	/// <param name="bUnit">BUnit where the path starts</param>
	/// <param name="bMapTile">BMapTile the goal of the path</param>
	public Path HighlightMovementPath(BUnit bUnit, BMapTile bMapTile)
	{
		// get path from unit to maptile with pathfinding algorithm
		Path path = controller.GetPath(bUnit.unit.mapTile, bMapTile.mapTile);
		for (int i = 0; i < path.Length; i++) {
			GetBMapTile(path[i]).ChangeColorState(BMapTile.ColorState.PATH);
		}
		return path;
	}

	/// <summary>
	/// Place a marker on the given BMapTile
	/// </summary>
	/// <param name="bMapTile">BMapTile.</param>
	public void SetMovementMarker (BMapTile bMapTile)
	{
		moveMarker.SetActive(true);
		moveMarker.transform.position = bMapTile.transform.position;
	}

	/// <summary>
	/// Change color of all MapTile to default.
	/// </summary>
	public void CleanMap ()
	{
		// reset color state of all maptiles
		foreach(BMapTile[] column in bMapTiles)
			foreach(BMapTile tile in column)
				tile.ChangeColorState(BMapTile.ColorState.DEFAULT);

		// hide moveMarker
		moveMarker.SetActive(false);
	}

	/// <summary>
	/// Ends the turn.
	/// </summary>
	public void EndTurn ()
	{
		controller.EndTurn();
	}

	/// <summary>
	/// Gets the BMapTile representation for a MapTile
	/// </summary>
	/// <returns>The BMapTile.</returns>
	/// <param name="mapTile">MapTile</param>
	public BMapTile GetBMapTile(MapTile mapTile) 
	{
		return bMapTiles[mapTile.x][mapTile.y];
	}

	/// <summary>
	/// Gets the BUnit representation for an Unit
	/// </summary>
	/// <returns>The BUnit.</returns>
	/// <param name="unit">Unit</param>
	public BUnit GetBUnit(Unit unit)
	{
		return bUnits.Where(t => t.unit == unit).FirstOrDefault();
	}

	public void OnDestroy()
	{
		// unregister from all Events
		EventProxyManager.Clear();
	}
}

public class EventDoneEvent : EventProxyArgs
{
	public EventDoneEvent() 
	{
		this.name = EventName.EventDone;
	}
}

public static class DisplayRangeMode
{
	public const int TEAM_0_CLICKABLE = 0;
	public const int TEAM_1_CLICKABLE = 1;
	public const int ALL_CLICKABLE = 2;
}