using UnityEngine;
using System.Collections;

public class GridMoveAnimation : MonoBehaviour {
	
	public Animation animation;
	public GameObject waypointGO;
	
	private bool run = false;
	private Vector3[] wps;
	private int nextWp;
	
	// Update is called once per frame
	void Update () {
		if( run ) {
			float distance = (wps[nextWp] - transform.position).magnitude;
			
			Vector3 move = (wps[nextWp] - wps[nextWp-1]);
			move = transform.InverseTransformDirection( move );
			transform.Translate( move * Time.deltaTime );
			
			if( Vector3.Dot(wps[nextWp] - transform.position,transform.forward) < 0 ) {
				transform.position = wps[nextWp];
				nextWp++;
				
				// we reached the last wp, stop moveing
				if( nextWp >= wps.Length ) {
					run = false;
					animation.Stop();
					animation.transform.rotation = Quaternion.identity;
					return;
				}
				
				transform.LookAt(wps[nextWp]);
				Debug.Log(nextWp);
			}
			
			
			
		}
	}
	
	/**
	 * Exspect each quad on the path to be an way point (wp)
	 */ 
	public void StartMoveAnimation(Vector3[] waypoints) {
		if( waypoints.Length <= 1 )
			return;
		wps = waypoints;
		nextWp = 1;
		transform.LookAt(wps[nextWp]);
		run = true;
		
		foreach(Vector3 wp in wps) {
			Instantiate(waypointGO,wp, Quaternion.identity);	
		}
		animation.Play();
	}
}
