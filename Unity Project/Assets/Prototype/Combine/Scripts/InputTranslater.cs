using UnityEngine;
using System.Collections;

public class InputTranslater : MonoBehaviour {
	
	public Map map;
	
	void Awake() {
		if( !map ) {
			Debug.LogError("No map reference was set! We can't proceed.");
			enabled = false;
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetButtonDown("Fire1") ) {
			// we clicked with mouse or tapped on the touchscreen
			
			// cast an ray from the screen point
			Ray cursorRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit[] hitList = Physics.RaycastAll( cursorRay );
			
			// let's see what we hit with the raycast
			foreach( RaycastHit hit in hitList ) {		
				if( hit.collider.CompareTag("GridQuad") ) {
					// we hit an quad of the map
					// get it's coordinates
					// x- and z-local coordinates are equivalent to map position (floored to int)
					Vector2 pos = new Vector2(
						Mathf.Floor(  hit.collider.transform.localPosition.x ),
						Mathf.Floor(  hit.collider.transform.localPosition.z )
						);
					
					// proceed tap to map
					map.TapOnQuad( pos );
					return;
				}
			}
		}
	}
}
