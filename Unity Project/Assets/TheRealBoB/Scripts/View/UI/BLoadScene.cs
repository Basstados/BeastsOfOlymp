using UnityEngine;
using System.Collections;

public class BLoadScene : MonoBehaviour {

	public string sceneName;

	public void OnClick()
	{
		Application.LoadLevel(sceneName);
	}
}
