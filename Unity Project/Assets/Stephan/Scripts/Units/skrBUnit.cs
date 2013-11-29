using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class skrBUnit : MonoBehaviour {

	public skrUnit unit;

	Queue<Move> moveQueue = new Queue<Move>();
	bool moveRoutineIsRunning = false;

	struct Move {
		public Vector3 start;
		public Vector3 end;

		public Move (UnityEngine.Vector3 start, UnityEngine.Vector3 end)
		{
			this.start = start;
			this.end = end;
		}
	}

	public void MoveUnit (Vector3 startPosition, Vector3 endPosition) 
	{
		moveQueue.Enqueue(new Move (startPosition, endPosition));
		// StartCoroutine(MoveRoutine (startPosition, endPosition));
		Debug.Log("From " + startPosition + " to " + endPosition);
	}

	void Update() {
		if (moveQueue.Count > 0 && !moveRoutineIsRunning) {
			// start coroutine
			StartCoroutine(MoveRoutine(moveQueue.Dequeue()));
		}
	}

	IEnumerator MoveRoutine(Move move) 
	{
		moveRoutineIsRunning = true; // prevet starting a 2nd routine

		transform.position = move.start;

		while(transform.position != move.end) {
			Vector3 translation = move.end - move.start;
			translation = translation.normalized * unit.speed * Time.deltaTime;
			if (Vector3.Distance(transform.position, move.end) >= translation.magnitude)
				transform.Translate( translation );
			else
				transform.position = move.end;

			yield return 0; // continue next frame
		}

		moveRoutineIsRunning = false; // free routine
	}
}
