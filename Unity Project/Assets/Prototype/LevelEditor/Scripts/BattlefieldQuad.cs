using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BattlefieldQuad : MonoBehaviour {
	
	public Material passableMat;
	public Material unpassableMat;
	
	public int movementPenalty = 1;
	
	// Update is called once per frame
	void Update () {
		if( !Application.isPlaying ) {
			// if penalty changes in editor mode, change material
			if( movementPenalty >= 0 ) {
				renderer.material = passableMat;	
			} else {
				renderer.material = unpassableMat;
			}
		}
	}
}
