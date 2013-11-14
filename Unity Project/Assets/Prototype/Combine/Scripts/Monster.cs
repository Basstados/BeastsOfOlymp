using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
	
	public bool controlable = true;
	public int movement_range = 5;
	public int max_hp = 10;
	
	public int hp;
	
	Color flashColor = Color.red;
	Color defaultColor;
	
	private Map map;
	private int[,] distanceMatrix;
	private Vector2 currentPos = Vector2.zero;
	
	// movement action
	private int[] pathDestionation = new int[]{-1,-1};
	private Vector3[] wps;
	
	private Attack attack;
	
	private Action currentAction = Action.IDLE;
	
	enum Action {
		IDLE,
		MOVE,
		ATTACK
	}

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		attack = new Attack("Tackle", this.gameObject );
		hp = max_hp;
		defaultColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitMove() {
		distanceMatrix = map.CalculateDistanceMatrix( currentPos, movement_range );
		map.DisplayDistanceMatrix( distanceMatrix, movement_range );
		currentAction = Action.MOVE;
	}
	
	public void PerformAction( Vector2 pos ) {
		switch( currentAction ) {
			case Action.MOVE:
				PerformMove( pos );
				break;
			case Action.ATTACK:
				PerformAttack( pos );
				break;
		}
	}
	
	public void PerformMove( Vector2 pos ) {
		if( pathDestionation[0] == (int) pos.x && pathDestionation[1] == (int) pos.y ) {
			Debug.Log("Move to " + pos);
			// we clicked target quad a 2nd time
			// now move there
			map.ResetMapDisplay();
			GetComponent<GridMoveAnimation>().StartMoveAnimation( wps );
			// update saved monster list
			Vector2 newPos = new Vector2( pathDestionation[0], pathDestionation[1] );
			map.MoveMonster( this, currentPos, newPos );
			currentPos = newPos;
			currentAction = Action.IDLE;
			map.ActionFinished();
		} else {
			// new quad in range clicked
			// find path
			map.ResetMapDisplay();
			map.DisplayDistanceMatrix( distanceMatrix, movement_range );
			List<int[]> path = map.CalculatePath( new int[]{ (int) pos.x, (int) pos.y } );
			pathDestionation = path[ path.Count-1 ];	
			wps = map.GetWaypoints( path );
		}	
	}
	
	public void InitAttack() {
		distanceMatrix = map.CalculateDistanceMatrix( currentPos, attack.Range );
		map.DisplayDistanceMatrix( distanceMatrix, attack.Range );
		currentAction = Action.ATTACK;
	}
	
	public void PerformAttack( Vector2 pos ) {
		Debug.Log("Attack on " + pos);
		// new quad in range clicked
		// find path
		map.ResetMapDisplay();
		Monster target = map.GetMonsterAt( pos );
		if( target ) {
			attack.Execute( target );
		}
		currentAction = Action.IDLE;
		map.ActionFinished();
	}
	
	public void TakeDamage(int dmg) {
		hp -= dmg;
		
		StartCoroutine(DamageFlash());
	}
	
	private IEnumerator DamageFlash() {
		renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		renderer.material.color = defaultColor;
		
		if( hp <= 0 ) {
			// enemy died
			// do something...
			renderer.enabled = false;
		}
	}

	public Vector2 CurrentPos {
		get {
			return this.currentPos;
		}
		set {
			currentPos = value;
		}
	}
}
