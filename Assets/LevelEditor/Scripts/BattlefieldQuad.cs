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
			// update penalty matrix
			// the parent gameobject always has the generation script attached (by generation)
			// the indices for the matrix are equal to our local coordiantes (by generation)
			transform.parent.GetComponent<LevelGeneration>().UpdatePenalty(
				(int) transform.localPosition.x,
				(int) transform.localPosition.z, 
				movementPenalty);
			
			// if penalty changes in editor mode, change material
			if( movementPenalty >= 0 ) {
				renderer.material = passableMat;	
			} else {
				renderer.material = unpassableMat;
			}
		}
	}
}
