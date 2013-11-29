using UnityEngine;
using System.Collections;

public class skrInputTranslater : MonoBehaviour {

	skrController controller;

	public event System.EventHandler<skrMapTileTappedEvent> MapTileTapped;
	void OnMapTileTapped(skrMapTile mapTile) {
		if (MapTileTapped != null)
			MapTileTapped (this, new skrMapTileTappedEvent(mapTile));
	}

	public void Init(skrController controller) {
		// TODO move the event registration to another place, doesn't make sense here :/
		this.controller = controller;
		MapTileTapped += controller.HandleMapTileTapped;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetButtonDown("Fire1") ) {
			// we clicked with mouse or tapped on the touchscreen
			
			// first we check if we hit some ui
			// if( RaycastCheckUI() )
			//	return;	
			
			// cast an ray from the screen point
			Ray cursorRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			
			//RaycastHit[] hitList = Physics.RaycastAll( cursorRay );
			RaycastHit hit = new RaycastHit();
			
			int mask = 1 << LayerMask.NameToLayer("Ignore Raycast");
			mask = ~mask;
			
			if( Physics.Raycast(cursorRay, out hit, Mathf.Infinity, mask) ) {
				
				// let's see what we hit with the raycast
				if( hit.collider.gameObject.layer == LayerMask.NameToLayer("UI 3D") ) {
					// we hit an ui element first
					// stop looking for map and stuff and just return
					return;
				}
				
				if( hit.collider.CompareTag("GridQuad") ) {
					// we hit an quad of the map

					skrBMapTile bMapTile = hit.transform.GetComponent<skrBMapTile>();
					OnMapTileTapped(bMapTile.mapTile);
					// proceed tap to map
					return;
				}
			}
		}
	}
}

public class skrMapTileTappedEvent : System.EventArgs {
	public skrMapTile mapTile;

	public skrMapTileTappedEvent (skrMapTile mapTile)
	{
		this.mapTile = mapTile;
	}
}
