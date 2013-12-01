using UnityEngine;
using System.Collections;

public class BInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1")) {
			OnTap();
		}
	}

	void OnTap() 
	{
		// we clicked with mouse or tapped on the touchscreen
		
		// first we check if we hit some ui
		// if( RaycastCheckUI() )
		//	return;	
		
		// cast an ray from the screen point
		Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit = new RaycastHit();
		// create layer mask to ignore layer "Ignore Raycast" and hit all others
		int mask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		mask = ~mask;
		
		if(Physics.Raycast(cursorRay, out hit, Mathf.Infinity, mask)) {
			// let's see what we hit with the raycast
			// TODO better name for UI Layer
			if( hit.collider.gameObject.layer == LayerMask.NameToLayer("UI 3D") ) {
				// we hit an ui element first
				// stop looking for map and stuff and just return
				return;
			}
			
			if( hit.collider.CompareTag("GridQuad") ) {
				// we hit an quad of the map
	
				// fire event for the tapped mapTile
				MapTile mapTile = hit.collider.GetComponent<BMapTile>().mapTile;
				EventProxyManager.FireEvent(EventName.MapTileTapped,this, new MapTileTappedEvent(mapTile));
			}
		}
	}
}


public class MapTileTappedEvent : System.EventArgs {
	public MapTile mapTile;
	
	public MapTileTappedEvent (MapTile mapTile)
	{
		this.mapTile = mapTile;
	}
}
