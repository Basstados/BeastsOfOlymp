using UnityEngine;
using System.Collections;

public class BNextLevel : MonoBehaviour {

	void OnEnable() {
		if(!PlayerPrefs.HasKey("LastLevel")) {
			PlayerPrefs.SetInt("LastLevel",0);
		}
		if(!PlayerPrefs.HasKey("HighestLevel")) {
			PlayerPrefs.SetInt("HighestLevel",0);
		}
	}

	public void OnClick() {
		int nextLevel = PlayerPrefs.GetInt("LastLevel") + 1;
		Application.LoadLevel("Level " + nextLevel);
	}
}
