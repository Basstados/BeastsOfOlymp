using UnityEngine;
using System.Collections;

public class BInputManager : MonoBehaviour {
	
	public InputPhase phase;

	public enum InputPhase {
		DEFAULT,
		PICKTARGET
	}

	private const int UNIT_LAYER = 9;
	private const int UI_LAYER = 8;
	private const int GRID_LAYER = 10;

	private BView bView;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1") && phase == InputPhase.PICKTARGET) {
			OnTap();
		}
	}

	void OnTap() 
	{
		// we clicked with mouse or tapped on the touchscreen
		
		// cast an ray from the screen point
		Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit = new RaycastHit();
		// create layer mask to ignore layer "Ignore Raycast" and hit all others
		int mask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		mask = ~mask;

		BMapTile bMapTile = null;

		if(Physics.Raycast(cursorRay, out hit, Mathf.Infinity, mask)) {
			// let's see what we hit with the raycast
			switch( hit.collider.gameObject.layer ) {
			case UI_LAYER:
				// we hit an ui element first
				// stop looking for map and stuff and just return
				return;
				break;
			case UNIT_LAYER:
				// find the quad the unit is standing on
				BUnit bUnit = hit.collider.GetComponent<BUnit>();
				bMapTile = BView.Instance.GetBMapTile(bUnit.unit.mapTile);
				// fire event for the tapped mapTile
				EventProxyManager.FireEvent(this, new BMapTileTappedEvent(bMapTile));
				break;
			case GRID_LAYER:
				// we hit an quad of the map
				bMapTile = hit.collider.GetComponent<BMapTile>();
				// fire event for the tapped mapTile
				EventProxyManager.FireEvent(this, new BMapTileTappedEvent(bMapTile));
				break;
			}
		}
	}
}


public class BMapTileTappedEvent : EventProxyArgs {
	public BMapTile bMapTile;
	
	public BMapTileTappedEvent (BMapTile bMapTile)
	{
		this.name = EventName.BMapTileTapped;
		this.bMapTile = bMapTile;
	}
}
