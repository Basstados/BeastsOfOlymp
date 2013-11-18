using UnityEngine;
using System.Collections;

public class QuadGenerator : MonoBehaviour {

	public GameObject quadPrefab;
	public float speed;
	
	public GUIText InstatiationCounter;
	
	public bool run = true;
	public int max_warnings = 3;
	int warnings = 0;
	
	int instantiation = 0;
	
	float lastInstatiation = 0f;
	// (di, dj) is a vector - direction in which we move right now
    int di = 1;
    int dj = 0;
	// length of current segment
	int segment_length = 1;
	// current position (i, j) and how much of current segment we passed
	int i = 0;
	int j = 0;
	int segement_passed = 0;
	
	void Update() {
		if( !run )
			return;
		
		if( lastInstatiation + 1/speed < Time.time ) {
			// time for the next instance
			
			GameObject handle = Instantiate(quadPrefab) as GameObject;
			handle.transform.position = new Vector3(i, 0,j);
			lastInstatiation = Time.time;
			instantiation++;
			InstatiationCounter.text = "Quads: " + instantiation;
			if( instantiation > 9000 ) {
				InstatiationCounter.text = "Quads:  > 9000";
				run = false;
			}
			
			// make a step, add 'direction' vector (di, dj) to current position (i, j)
			i += di;
			j += dj;
			segement_passed++;
			
			if( segement_passed == segment_length ) {
				// done with current segment
				segement_passed = 0;
				
				// 'rotate' directions
				int buffer = di;
				di = -dj;
				dj = buffer;
				
				// increse segement length if necessary
				if( dj == 0 ) {
					segment_length++;	
				}
			}	
		}
	}
	
	
	public void WarningMsg() {
		warnings++;
		Debug.Log("Recieved Warning");
		if( warnings > max_warnings ) {
			run = false;	
		}
	}
}
