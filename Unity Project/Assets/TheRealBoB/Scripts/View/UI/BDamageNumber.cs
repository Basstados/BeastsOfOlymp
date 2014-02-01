using UnityEngine;
using System.Collections;

public class BDamageNumber : MonoBehaviour {

	public UILabel label;

	public void Display(string text) 
	{
		label.text = text; 
//		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
//		float x = screenPos.x / Screen.width;
//		float y = screenPos.y / Screen.height;
//		screenPos = new Vector3(x * 1150f, y * 720f,0);
//		transform.localPosition = screenPos;
		label.gameObject.GetComponent<Animation>().Play();
	}
}
