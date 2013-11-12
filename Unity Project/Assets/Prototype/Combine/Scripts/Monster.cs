using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	
	public int movement_range = 5;
	
	private Map map;
	private int[,] distanceMatrix;
	private Vector2 currentPos = Vector2.zero;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitMove() {
		distanceMatrix = map.CalculateDistanceMatrix( currentPos, movement_range );
		map.DisplayDistanceMatrix( distanceMatrix, movement_range );
	}

	public Vector2 CurrentPos {
		get {
			return this.currentPos;
		}
		set {
			currentPos = value;
		}
	}
}
