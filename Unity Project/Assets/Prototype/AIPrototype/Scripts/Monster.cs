using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
	
	public bool controlable = true;
	public int movement_range = 5;
	public int speed = 4;
	public int max_hp = 10;
	
	public int hp;

	public GameObject renderObject;
	public CombatMenu combatMenu;

	public Team team;

	public enum Team {
		PLAYER,
		AI
	}
	
	Color flashColor = Color.red;
	Color defaultColor;
	
	private Prototype_Map map;
	private int[,] distanceMatrix;
	private Vector2 currentPos = Vector2.zero;
	
	// movement action
	private int[] pathDestionation = new int[]{-1,-1};
	private Vector3[] wps;
	
	private Prototype_Attack attack;
	
	private Action currentAction = Action.IDLE;

	// flags used during turn to make sure player dosn't move twice
	public bool hasMoved = false;
	public bool hasAttacked = false;
	
	public enum Action {
		IDLE,
		MOVE,
		ATTACK
	}

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Prototype_Map>();
		attack = new Prototype_Attack("Tackle", this.gameObject );
		hp = max_hp;
		defaultColor = renderObject.renderer.material.color;
	}
	
	public void InitMove() {
		if( !hasMoved ) {
			distanceMatrix = map.CalculateDistanceMatrix( currentPos, movement_range );
			map.DisplayDistanceMatrix( distanceMatrix, movement_range );
			currentAction = Action.MOVE;
		}
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
			hasMoved = true;
			map.ActionFinished();

			// reset path destination
			pathDestionation = new int[]{9000,9000};
			ContinueTurn();
		} else {
			// new quad in range clicked
			// find path
			map.ResetMapDisplay();
			map.DisplayDistanceMatrix( distanceMatrix, movement_range );
			List<int[]> path = map.CalculatePath(new int[]{(int) currentPos.x,(int) currentPos.y } , new int[]{ (int) pos.x, (int) pos.y } );
			foreach( int[] p in path ) {
				Debug.Log("WP: " + p[0] + "," + p[1]);
			}
			pathDestionation = path[ path.Count-1 ];
			wps = map.GetWaypoints( path );
		}	
	}
	
	public void InitAttack() {
		if( !hasAttacked ) {
			distanceMatrix = map.CalculateDistanceMatrixAttackMode( currentPos, attack.Range);
			map.DisplayDistanceMatrix( distanceMatrix, attack.Range );
			currentAction = Action.ATTACK;
		}
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
		hasAttacked = true;

		if( team == Team.PLAYER )
			ContinueTurn();
	}

	public void StartTurn() {
		if( hp <= 0 ) {
			Prototype_Combat.Instance.NextTurn();
		}
		//TODO
		if( team == Team.AI ) {
			GetComponent<PrototypeArtificialIntelligence>().StartCoroutine("DoTurn", movement_range);
		} else {
			hasMoved = false;
			hasAttacked = false;

			Vector3 worldPos = map.MapToWorldPosition( currentPos );
			Vector3 screenPos = Camera.main.WorldToScreenPoint( worldPos );
			combatMenu.OpenForMonster( screenPos, this );
		}
	}

	private void ContinueTurn() {
		if( hasMoved && hasAttacked ) {
			// all actions are done; end this turn
			Prototype_Combat.Instance.NextTurn();
		} else {
			// turn not over yet; reopen combat menu
			Vector3 worldPos = map.MapToWorldPosition( currentPos );
			Vector3 screenPos = Camera.main.WorldToScreenPoint( worldPos );
			combatMenu.OpenForMonster( screenPos, this );
		}
	}
	
	public void TakeDamage(int dmg) {
		hp -= dmg;
		
		StartCoroutine(DamageFlash());
	}
	
	private IEnumerator DamageFlash() {
		renderObject.renderer.material.color = flashColor;
		yield return new WaitForSeconds(0.5f);
		renderObject.renderer.material.color = defaultColor;
		
		if( hp <= 0 ) {
			// monster died
			// do something...
			Debug.Log("I'm Dead!");
			map.RemoveMonster( currentPos );
			renderObject.renderer.enabled = false;
			transform.position = new Vector3(9000,0,9000);
			Prototype_Combat.Instance.KillMonster( this );
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

	public Action CurrentAction {
		get {
			return currentAction;
		}
		set {
			currentAction = value;
		}
	}

	public Prototype_Attack Attack {
		get {
			return attack;
		}
	}
}
