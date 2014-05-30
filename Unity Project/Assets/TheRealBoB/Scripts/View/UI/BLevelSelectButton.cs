using UnityEngine;
using System.Collections;

public class BLevelSelectButton : MonoBehaviour {

	public int level;

	void OnEnable() {
		if(!PlayerPrefs.HasKey("LastLevel")) {
			PlayerPrefs.SetInt("LastLevel",0);
		}
		if(!PlayerPrefs.HasKey("HighestLevel")) {
			PlayerPrefs.SetInt("HighestLevel",0);
		}

		if(PlayerPrefs.GetInt("HighestLevel") + 1 < level) {
			GetComponent<UIButton>().isEnabled = false;
		}
	}

	void OnDisable() {
		GetComponent<UIButton>().isEnabled = true;
	}
	
	public void OnClick() {
		Application.LoadLevel("Level " + level);
	}
}
