using UnityEngine;
using System.Collections;

public class ToggleButton : MonoBehaviour {

	public GameObject target;

	public void OnClick() {
		target.SetActive(!target.activeSelf);
	}
}
