using UnityEngine;
using System.Collections;

public class BCameraMover : MonoBehaviour {

	public float minSpeed = 2;

	Vector3 target;
	bool routineIsRunning = false;

	public void Focus(GameObject go) {
		target = go.transform.position;

		if(!routineIsRunning)
			StartCoroutine(SmoothMoveRoutine());
	}

	IEnumerator SmoothMoveRoutine() {
		routineIsRunning = true;
		Vector3 translation;
		float distance;

		while(transform.position != target) {
			translation = target - transform.position;
			distance = translation.magnitude;
			float factor = (distance > minSpeed) ? distance : minSpeed;
			translation = translation.normalized * Time.deltaTime * factor;
			if(translation.magnitude > distance) {
				transform.position = target;
				break;
			}
			transform.Translate(translation);
			yield return 0;
		}
		routineIsRunning = false;
	}
}
