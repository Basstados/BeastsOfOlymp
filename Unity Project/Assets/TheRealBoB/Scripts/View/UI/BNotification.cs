using UnityEngine;
using System.Collections;

public class BNotification : MonoBehaviour {

	public GameObject notifyGO;
	public UILabel textLabel;


	public void Display(string text)
	{
		notifyGO.SetActive(true);
		textLabel.text = text;
	}
}
