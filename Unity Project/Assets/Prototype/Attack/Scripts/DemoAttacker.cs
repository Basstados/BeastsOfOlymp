using UnityEngine;
using System.Collections;

public class DemoAttacker : MonoBehaviour {

	public Material reachableMat;
	
	Range range;
	BattlefieldQuad[,] battleMatrix;
//	Prototype_Attack myAttack;
	
	// Use this for initialization
	void Start () {
		
	}
	
	private void Test() {
		if( battleMatrix == null ) {
			GameObject[,] quadMatrix = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>().QuadMatrix;
			battleMatrix = new BattlefieldQuad[quadMatrix.GetLength(0), quadMatrix.GetLength(1)];
			
			for(int i=0; i<quadMatrix.GetLength(0); i++) {
				for(int j=0; j<quadMatrix.GetLength(1); j++) {
					battleMatrix[i,j] = quadMatrix[i,j].GetComponent<BattlefieldQuad>();
				}
			}
		}
		
		int[] pos = new int[]{(int)transform.position.x, (int)transform.position.z};
		
		range = new Range(battleMatrix, 6);
		range.UpdateCalculations(pos);
		
		//DisplayRange( range.DistanceMatrix );
		
//		myAttack = new Prototype_Attack(2, range.intValue, this.gameObject );
		// myAttack = new Attack("Tackle", battleMatrix, this.gameObject);
		// myAttack.Range.UpdateCalculations( new int[]{ (int) transform.position.x, (int) transform.position.z});
		// DisplayRange( myAttack.Range.DistanceMatrix );
	}
	
	private void DisplayRange(int[,] distanceMatrix) {
		for(int i=0; i<distanceMatrix.GetLength(0); i++) {
			for(int j=0; j<distanceMatrix.GetLength(1); j++) {
				if( distanceMatrix[i,j] < range.intValue ) {
					// this quad is in range
					battleMatrix[i,j].renderer.material = reachableMat;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.time < 2 ) {
			Test();	
		}
		
		
		if(Input.GetButtonDown("Fire1")) {
			Ray cursorRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit[] hitList = Physics.RaycastAll( cursorRay );
			
			foreach( RaycastHit hit in hitList ) {
				if( hit.collider.CompareTag("Enemy") ) {
					// you clicked an enemy, lets attack it!
					
//					myAttack.Execute( hit.collider.gameObject );
					break;
				}
			}
		}
	}
}
