using UnityEngine;
using System.Collections;

public class BUITooglePanel : MonoBehaviour {

	public UIPanel panel;

	public void OnClick() 
	{
		panel.gameObject.SetActive(!panel.gameObject.activeSelf);
	}
}
