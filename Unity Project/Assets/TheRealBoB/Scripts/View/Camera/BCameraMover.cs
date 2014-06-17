using UnityEngine;
using System.Collections;

public class BCameraMover : MonoBehaviour {

	public float movementDamping = 3.0f;
	public float minSpeed = 0.3f;
	public float threshold = 0.1f;
	public float maxX = 15;
	public float minX = -5;
	public float maxZ = 15;
	public float minZ = -5;

	Vector3 target;
	public Vector3 Target {
		get {
			return target;
		}
		set {
			// ensure the focus target is inside the camera move area
			target = value;
			target.x = Mathf.Clamp(target.x, minX, maxX);
			target.y = 0f;
			target.z = Mathf.Clamp(target.z, minZ, maxZ);
		}
	}

	void Awake() 
	{
		target = this.gameObject.transform.position;
	}

	public void Focus(Vector3 pos) 
	{
		Target = pos;
	}

	void Update() {
		float distance = (target - transform.position).magnitude;
		if(distance >= threshold) {
			transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * movementDamping);
		}
	}
}
