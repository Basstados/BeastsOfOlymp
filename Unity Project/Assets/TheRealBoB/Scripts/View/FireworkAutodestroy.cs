using UnityEngine;
using System.Collections;

public class FireworkAutodestroy : MonoBehaviour {

	public float duration;
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if(duration < 0) {
			Destroy(this.gameObject);
		}
	}
}
