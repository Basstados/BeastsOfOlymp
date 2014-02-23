using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTileDataInput : MonoBehaviour {

	int width = 10;
	int height = 10;
	char[] trimChar = new char[]{'|'};

	byte[][] penalties;
	UIInput[][] mapTileInputs;

	public UIInput widthInput;
	public UIInput heightInput;
	public GameObject fieldInputPrefab;
	public int fieldSize = 25;

	public void Init(byte[][] penalties) {
		width = penalties.Length;
		height = penalties[0].Length;
		this.penalties = penalties;

		widthInput.value = width.ToString();
		heightInput.value = height.ToString();
		GenerateInputFields();

		for (int i = 0; i < penalties.Length; i++) {
			for (int j = 0; j < penalties[i].Length; j++) {
				mapTileInputs[i][j].value = penalties[i][j].ToString();
			}
		}
	}

	public void GenerateInputFields() 
	{
		// clear children
		List<GameObject> children = new List<GameObject>();
		foreach(Transform child in transform) children.Add(child.gameObject);
		foreach(GameObject child in children) Destroy(child);



		width = int.Parse(widthInput.value.TrimEnd(trimChar));
		height = int.Parse(heightInput.value.TrimEnd(trimChar));

		mapTileInputs = new UIInput[width][];

		for (int i = 0; i < width; i++) {
			mapTileInputs[i] = new UIInput[height];
			for (int j = 0; j < height; j++) {
				GameObject handle = (GameObject) Instantiate(fieldInputPrefab);
				handle.name = "Field [" + i + "," + j + "]";
				handle.transform.parent = this.transform;
				handle.transform.localPosition = new Vector3( fieldSize * (i+1), - fieldSize * (j+1),0);
				handle.transform.localScale = Vector3.one;

				mapTileInputs[i][j] = handle.GetComponent<UIInput>();
			}
		}
	}

	public byte[][] GetPenalties ()
	{
		penalties = new byte[mapTileInputs.Length][];

		for (int i = 0; i < mapTileInputs.Length; i++) {
			penalties[i] = new byte[mapTileInputs[i].Length];
			for (int j = 0; j < mapTileInputs[i].Length; j++) {
				penalties[i][j] = byte.Parse(mapTileInputs[i][j].value.TrimEnd(trimChar));
			}
		}

		return penalties;
	}
}