using UnityEngine;
using System.Collections;

public class InputTranslater : MonoBehaviour {
	
	public Prototype_Map map;
	public Camera uiCamera;
	
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
			
			// first we check if we hit some ui
			if( RaycastCheckUI() )
				return;	
			
			// cast an ray from the screen point
			Ray cursorRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			
			//RaycastHit[] hitList = Physics.RaycastAll( cursorRay );
			RaycastHit hit = new RaycastHit();
			
			int mask = 1 << LayerMask.NameToLayer("Ignore Raycast");
			mask = ~mask;
			
			if( Physics.Raycast(cursorRay, out hit, Mathf.Infinity, mask) ) {
			
			// let's see what we hit with the raycast
			// foreach( RaycastHit hit in hitList ) {		
			//for( int i=hitList.Length-1; i>=0; i--) {
				//RaycastHit hit = hitList[i];
				Debug.Log( hit.collider.name );
				if( hit.collider.gameObject.layer == LayerMask.NameToLayer("UI 3D") ) {
					// we hit an ui element first
					// stop looking for map and stuff and just return
					return;
				}
				
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
	
	/**
	 * Do a raycast on UI camera limited to user interface layer and check for collision.
	 * Return true if some ui element was hit, return false else.
	 */ 
	private bool RaycastCheckUI() {
		Ray ray = uiCamera.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit = new RaycastHit();
		
		int layerMask = 1 << LayerMask.NameToLayer("User Interface");
		
		if( Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) ) {
			return true;	
		} else {
			return false;
		}
	}
}

