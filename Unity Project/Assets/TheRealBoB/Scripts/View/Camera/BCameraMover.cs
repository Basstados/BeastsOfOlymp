using UnityEngine;
using System.Collections;

public class BCameraMover : MonoBehaviour {

	public float movementDamping = 3.0f;
	public float minSpeed = 0.3f;
	public float threshold = 0.1f;

	GameObject target;
	bool routineIsRunning = false;

	void Awake() 
	{
		target = this.gameObject;
	}

	public void Focus(GameObject go) 
	{
		target = go;
	}

	void Update() {
		float distance = (target.transform.position - transform.position).magnitude;
		if(distance < threshold) {
			Vector3 translation = (target.transform.position - transform.position).normalized * Time.deltaTime * minSpeed;
		} else {
			transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * movementDamping);
		}
	}
}
