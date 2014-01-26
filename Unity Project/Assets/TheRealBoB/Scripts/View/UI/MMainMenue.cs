using UnityEngine;
using System.Collections;

public class MMainMenue : MonoBehaviour {

	public void OnStartGameClick() 
	{
		Application.LoadLevel ("testScene");

	}

	public void OnCreditsClick() 
	{
		//Application.LoadLevel ("Credits");
		Debug.Log("Credits Button Clicked");
		
	}

	public void OnExitGameClick() 
	{
		Application.Quit();
		
	}
}
