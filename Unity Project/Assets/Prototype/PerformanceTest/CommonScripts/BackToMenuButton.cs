using UnityEngine;
using System.Collections;

public class BackToMenuButton : MonoBehaviour {

	public void BackToMenu() {
		Application.LoadLevel("performanceTest_menu");	
	}
}
