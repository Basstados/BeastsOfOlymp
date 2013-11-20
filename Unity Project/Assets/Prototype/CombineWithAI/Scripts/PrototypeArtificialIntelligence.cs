using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrototypeArtificialIntelligence : MonoBehaviour {

	public List<Monster> enemies;
	public Map map;
	public UISlider slider;


	Monster target;
	Monster puppet;

	float timerProgress = 0.0f;
	float delay = 6f;

	bool run = true;

	void Start() {
		puppet = GetComponent<Monster>();
		ChooseTargetMonster();
		StartCoroutine("TurnLoop");
	}

	void Update() {
		if( run ) {
			timerProgress += Time.deltaTime;
			slider.value = timerProgress / delay;
		}
	}

	IEnumerator TurnLoop() {
		while(run) {
			timerProgress = 0.0f;

			yield return new WaitForSeconds( delay / 2 );
			// do AI turn
			ChooseAttack();

			yield return new WaitForSeconds( delay / 2 );
			ChooseTargetMonster();
			ChooseMoveDestination();
		}
	}

	public void ChooseTargetMonster() {
		if(enemies.Count <= 0) {
			Debug.Log("GameOver");
			run = false;
			StopCoroutine("TurnLoop");
		}

		// if no target selected; pick random one
		if( !target ) {
			target = enemies[ Random.Range(0, enemies.Count) ];
		}
		// if target is dead; choose new random one
		if( target.hp <= 0 ) {
			enemies.Remove( target );
			target = enemies[0];
		}
		// else continue on current target
		Debug.Log("Target is: " + target + target.CurrentPos);
	}

	public void ChooseMoveDestination() {
		int range = puppet.movement_range;

		int[] myPos = new int[2]{(int) puppet.CurrentPos.x, (int) puppet.CurrentPos.y};
		int[] targetPos = new int[2]{(int) target.CurrentPos.x, (int) target.CurrentPos.y};
		List<int[]> path = map.GetShortestPath( myPos, targetPos );

		if( path.Count <= 1 ) {
			// we are close enough; dont move
			map.ResetMapDisplay();
			puppet.CurrentAction = Monster.Action.IDLE;
			map.ActionFinished();
			return;
		}

		string str = "";
		foreach( int[] p in path ) {
			str += "[" + p[0] + "," + p[1] + "]";
		}
		Debug.Log(str);

		int[] destination = new int[2];
		if(range < path.Count) {
			destination = path[range-2];
			path = path.GetRange(0, range-1);
		} else {
			destination = path[path.Count-2];
			path = path.GetRange(0, path.Count-1);
		}
		Debug.Log("Move to: " + destination[0] + "." + destination[1] );


		map.ResetMapDisplay();
		Vector3[] wps = map.GetWaypoints( path );
		GetComponent<GridMoveAnimation>().StartMoveAnimation( wps );
		// update saved monster list
		Vector2 oldPos = new Vector2( myPos[0], myPos[1] );
		Vector2 newPos = new Vector2( destination[0], destination[1] );
		map.MoveMonster( puppet, puppet.CurrentPos, newPos );
		puppet.CurrentPos = newPos;
		puppet.CurrentAction = Monster.Action.IDLE;
		map.ActionFinished();
	}

	public void ChooseAttack() {
		Debug.Log("Try to attack!");
		Attack att = puppet.Attack;

		int[,] distanceMatrix = map.CalculateDistanceMatrix( puppet.CurrentPos, att.Range );
		if( distanceMatrix[(int)target.CurrentPos.x, (int)target.CurrentPos.y] <= att.Range ) {
			// monster is in range
			puppet.PerformAttack( target.CurrentPos );
		}
	}
}
