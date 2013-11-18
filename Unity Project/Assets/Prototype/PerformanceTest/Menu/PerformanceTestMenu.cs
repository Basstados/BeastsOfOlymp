using UnityEngine;
using System.Collections;

public class PerformanceTestMenu : MonoBehaviour {

	public void CheapQuadTest() {
		Application.LoadLevel("cheapQuadTest");	
	}
	
	public void LightedQuadTest() {
		Application.LoadLevel("quadsAndLight");	
	}
}
