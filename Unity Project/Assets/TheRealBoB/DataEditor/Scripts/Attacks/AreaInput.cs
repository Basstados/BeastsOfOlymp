using UnityEngine;
using System.Collections.Generic;

public class AreaInput : MonoBehaviour {

	public GameObject fieldToggle;
	public GameObject centerToggle;
	public float fieldToggleSize;
	public int areaSize;

	private UIToggle[,] fields;

	// Use this for initialization
	void Awake () {
		// we need a true center field so areaSize must be odd
		if(areaSize % 2 == 0) {
			Debug.LogError("areaSize must be odd!");
			return;
		}

		fields = new UIToggle[areaSize,areaSize];
		for (int i = 0; i < areaSize; i++) {
			for (int j = 0; j < areaSize; j++) {
				// we want a different toggle fiel in the center which indicate the click position
				GameObject go;
				if(i == j && i == areaSize / 2) {
					go = (GameObject) Instantiate(centerToggle);

				} else {
					go = (GameObject) Instantiate(fieldToggle);
				}
				go.transform.parent = this.transform;
				go.transform.localPosition = new Vector3(i*fieldToggleSize, -j*fieldToggleSize);
				go.transform.localScale = Vector3.one;
				fields[i,j] = go.GetComponent<UIToggle>();
			}
		}
	}

	public void Refresh(Vector[] area) {
		foreach(UIToggle field in fields) {
			field.value = false;
		}
		if(area != null)
			foreach(Vector pt in area) {
				int i = areaSize / 2 + pt.x;
				int j = areaSize / 2 + pt.y;
				fields[i,j].value = true;
			}
	}

	public Vector[] GetArea() {
		List<Vector> area = new List<Vector>();
		for (int j = 0; j < fields.GetLength(1); j++) {
			for (int i = 0; i < fields.GetLength(0); i++) {
				if(fields[i,j].value) {
					int x = i - areaSize / 2;
					int y = j - areaSize / 2;
					area.Add(new Vector(x,y));
				}
			}
		}

		return area.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
