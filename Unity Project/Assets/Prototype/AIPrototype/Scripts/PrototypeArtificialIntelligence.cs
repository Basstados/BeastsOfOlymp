using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Algorithms;

public class PrototypeArtificialIntelligence : MonoBehaviour {

	public List<Monster> enemies;
	public Prototype_Map map;
	public UISlider slider;


	Monster target;
	Monster puppet;

	float timerProgress = 0.0f;
	float delay = 6f;

	public bool run = true;

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

	public IEnumerator DoTurn(int range) {
		run = true;
		timerProgress = 0.0f;
		delay = range;
		ChooseTargetMonster();
		ChooseMoveDestination();
		yield return new WaitForSeconds( range );
		ChooseAttack();

		run = false;
		// all actions are done; end this turn
		Prototype_Combat.Instance.NextTurn();
	}

	public void ChooseTargetMonster() {
		if(enemies.Count <= 0) {
			Debug.Log("GameOver");
			run = false;
			StopCoroutine("TurnLoop");
		}
		// choose closed monster
		int minDist = int.MaxValue;
		int dst = 0;
		foreach( Monster enemy in enemies ) {
			dst = (int)( Mathf.Abs(enemy.CurrentPos.x - puppet.CurrentPos.x) + Mathf.Abs(enemy.CurrentPos.y - puppet.CurrentPos.y));
			if( dst < minDist ) {
				minDist = dst;
				target = enemy;
			}
		}

		// else continue on current target
		Debug.Log("Target is: " + target + target.CurrentPos);
	}

	public void ChooseMoveDestination() {
		int range = puppet.movement_range;

		Point myPos = new Point((int) puppet.CurrentPos.x, (int) puppet.CurrentPos.y);
		Point targetPos = new Point((int) target.CurrentPos.x, (int) target.CurrentPos.y);
		
		// find closed field to me, next to target
		/*TestPoint targetField = targetPos;
		int minDst = int.MaxValue;

		// get list of all neighbour fields for targetPos
		int[] x = new int[]{targetPos.x + 1, targetPos.x, targetPos.x - 1, targetPos.x};
		int[] y = new int[]{targetPos.y, targetPos.y + 1, targetPos.y, targetPos.y - 1};

		for( int i=0; i<x.Length; i++ ) {
			// check if field is inside map
			if( x[i] > 0 && x[i] < map.QuadMatrix.GetLength(0)
			   && y[i] > 0 && y[i] < map.QuadMatrix.GetLength(1) ) {
				// calulcate distance with infinty norm
				int dst = Mathf.Abs( x[i] - myPos.x ) + Mathf.Abs( y[i] - myPos.y );
				if( dst < minDst ) {
					targetField = new TestPoint(x[i], y[i]);
					minDst = dst;
				}
			}
		}*/


		byte[,] grid = new byte[map.QuadMatrix.GetLength(0),map.QuadMatrix.GetLength(1)];
		for( int i=0; i< map.QuadMatrix.GetLength(0); i++ ) {
			for( int j=0; j<map.QuadMatrix.GetLength(1); j++ ) {
				grid[i,j] = (map.QuadMatrix[i,j].penalty > 0) ? (byte) 1 : (byte) 0;
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

		if( path.Count <= 1 ) {
			// we are close enough; dont move
			puppet.CurrentAction = Monster.Action.IDLE;
			map.ActionFinished();
			return;
		}

		List<int[]> intPath = new List<int[]>();
		string str = "";
		foreach( PathFinderNode p in path ) {
			str += "[" + p.X + "," + p.Y + "]";
			intPath.Add( new int[2]{ p.X, p.Y } );
		}
		Debug.Log(str);

		int[] destination = new int[2];
		if(range < intPath.Count) {
			destination = intPath[range-1];
			intPath = intPath.GetRange(0, range);
		} else {
			destination = intPath[intPath.Count-2];
			intPath = intPath.GetRange(0, intPath.Count-1);
		}
		Debug.Log("Move to: " + destination[0] + "." + destination[1] );

		Vector3[] wps = map.GetWaypoints( intPath );
		str = "WPS: ";
		foreach( Vector3 wp in wps ) {
			str += wp + " ";
		}
		Debug.Log( str );
		GetComponent<GridMoveAnimation>().StartMoveAnimation( wps );
		// update saved monster list
//		Vector2 oldPos = new Vector2( myPos.x, myPos.y );
		Vector2 newPos = new Vector2( destination[0], destination[1] );
		map.MoveMonster( puppet, puppet.CurrentPos, newPos );
		puppet.CurrentPos = newPos;
		puppet.CurrentAction = Monster.Action.IDLE;
		map.ActionFinished();
	}

	public void ChooseAttack() {
		Debug.Log("Try to attack!");
		Prototype_Attack att = puppet.Attack;

		int[,] distanceMatrix = map.CalculateDistanceMatrixAttackMode( puppet.CurrentPos, att.Range );
		if( distanceMatrix[(int)target.CurrentPos.x, (int)target.CurrentPos.y] <= att.Range ) {
			// monster is in range
			puppet.PerformAttack( target.CurrentPos );
		}
	}
}
