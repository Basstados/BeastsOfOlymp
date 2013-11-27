using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ArenaMap : MonoBehaviour {

	public int rows;
	public int cols;

	private byte[,] grid;

	void Awake() {
		// instatiate field matrix on awake

		grid = new byte[rows,cols];

		for( int i=0; i<rows; i++ ) {
			for( int j=0; j<cols; j++ ) {
				grid[i,j] = 1;
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnDrawGizmos() {
		// draw grid
		Gizmos.color = Color.gray;
		Vector3 offset = transform.position;

		// draw a line for each row
		for( int i=0; i<rows+1; i++ ) {
			Gizmos.DrawLine( new Vector3(0, 0, i) + offset, new Vector3(cols, 0, i) + offset );
		}
		// draw a line for each column
		for( int i=0; i<cols+1; i++ ) {
			Gizmos.DrawLine( new Vector3(i ,0,  0) + offset, new Vector3(i, 	0, rows) + offset );
		}

		// draw points in the center of each field
		Gizmos.color = Color.white;

		for( int i=0; i<rows; i++ ) {
			for( int j=0; j<cols; j++ ) {
				Gizmos.DrawWireSphere( new Vector3( i+0.5f, 0, j+0.5f ), 0.05f );
			}
		}
	}
}
