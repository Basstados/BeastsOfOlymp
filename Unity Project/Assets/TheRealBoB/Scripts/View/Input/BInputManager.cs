using UnityEngine;
using System.Collections;

public class BInputManager : MonoBehaviour {

	public Camera uiCamera;
	public BCameraMover cameraMover;
	public float tapLength = 0.3f;
	public InputPhase phase;
	public AudioSource tapFieldSound;


	public enum InputPhase {
		DEFAULT,
		PICKTARGET
	}

	private const int UNIT_LAYER = 9;
	private const int UI_LAYER = 8;
	private const int GRID_LAYER = 10;
	private const int PLANE_LAYER = 11;
	
	private float tapTime = 0f;
	private Vector3 lastMousePosition = Vector3.zero;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{
		// reset tap time on tap start (button down)
		if(Input.GetButtonDown("Fire1")) {
			tapTime = 0f;
		}

		// track tap time while button is down
		if(Input.GetButton("Fire1")){
			tapTime += Time.deltaTime;
			if(tapTime > tapLength) {
				// now we assume it's a gesture not a tap
				OnHold();
			}
			//if( (lastMousePosition - Input.mousePosition).magnitude > 100) {
				lastMousePosition = Input.mousePosition;
			//}
		}

		if(Input.GetButtonUp("Fire1") && tapTime < tapLength) {
			if(phase == InputPhase.PICKTARGET) {
				OnTap();
			}
		}
	}

	void OnHold()
	{
		Vector3 lastMousePlanePos = ScreenToPlane(lastMousePosition);
		Vector3 currentMousePos = ScreenToPlane(Input.mousePosition);

		Vector3 targetPos = cameraMover.Target + (currentMousePos - lastMousePlanePos);
		cameraMover.Focus(targetPos);
	}

	Vector3 ScreenToPlane(Vector3 screenPoint)
	{
		// cast an ray from the screen point
		Vector3 mousePos = new Vector3(Screen.width, Screen.height) - screenPoint;
		Ray cursorRay = Camera.main.ScreenPointToRay(mousePos);
		
		RaycastHit hit = new RaycastHit();
		int mask = 1 << PLANE_LAYER;
		Physics.Raycast(cursorRay, out hit, Mathf.Infinity, mask);
		return hit.point;
	}

	void OnTap() 
	{
		// we clicked with mouse or tapped on the touchscreen
		
		// cast an ray from the screen point
		Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// create layer mask to ignore layer "Ignore Raycast" and hit all others
		int mask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		mask = ~mask;

		BMapTile bMapTile = null;

		RaycastHit[] hits = Physics.RaycastAll(cursorRay, Mathf.Infinity, mask);

		// this does not work since buttons will be Deactivate just as we cast the ray
		// so we don't hit them anymore :(
		if(CheckUIHit()) {
			return;
		}

		// check all objects hit on raycast
		foreach(RaycastHit hit in hits)  {
			// let's see what we hit with the raycast
			switch( hit.collider.gameObject.layer ) {
			case UI_LAYER:
				// we hit an ui element first
				// stop looking for map and stuff and just return
				Debug.LogWarning("Hit UI! Stop here");
				return;
			case UNIT_LAYER:
				// find the quad the unit is standing on
				BUnit bUnit = hit.collider.GetComponent<BUnit>();
				// fire event for the tapped bUnit
				tapFieldSound.Play();
				EventProxyManager.FireEvent(this, new BUnitTappedEvent(bUnit));
				break;
			case GRID_LAYER:
				// we hit an quad of the map
				bMapTile = hit.collider.GetComponent<BMapTile>();
				// fire event for the tapped mapTile
				tapFieldSound.Play();
				EventProxyManager.FireEvent(this, new BMapTileTappedEvent(bMapTile));
				break;
			}
		}
	}

	private bool CheckUIHit()
	{
		Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int mask = 1 << UI_LAYER;
		Debug.DrawRay(ray.origin,ray.direction);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
			return (hit.collider.gameObject.layer == UI_LAYER);
		} else {
			return false;
		}
	}
}

public class BUnitTappedEvent : EventProxyArgs {
	public BUnit bUnit;

	public BUnitTappedEvent (BUnit bUnit)
	{
		this.name = EventName.BUnitTapped;
		this.bUnit = bUnit;
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
