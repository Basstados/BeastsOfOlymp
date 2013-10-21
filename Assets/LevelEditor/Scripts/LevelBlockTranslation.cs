using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelBlockTranslation : MonoBehaviour {
	
	private int lockX;
	private int lockZ;
	private float lastY;
	
	void Start() {
		lockX = Mathf.FloorToInt( transform.localPosition.x );
		lockZ = Mathf.FloorToInt( transform.localPosition.z );
		lastY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		// Check if we moved away from lock or y-coordinate has changed
		if( !transform.localPosition.Equals( new Vector3(lockX, lastY, lockZ)) ) {
			// Some coordinate has changed
			// Restore x and z and floor y (on 1/4s)
			transform.localPosition = new Vector3(lockX, Mathf.Floor( transform.localPosition.y * 4 )/4f, lockZ);
		}
	}
}
