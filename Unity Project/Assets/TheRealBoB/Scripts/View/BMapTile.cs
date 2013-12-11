using UnityEngine;
using System.Collections;

public class BMapTile : MonoBehaviour {

	public MapTile mapTile;
	public ColorState colorState;

	Color defaultColor;

	public enum ColorState {
		INRANGE,
		DEFAULT
	}

	void Awake() 
	{
		defaultColor = renderer.material.color;
	}

	public void ChangeColorState(BMapTile.ColorState colorState)
	{
		this.colorState = colorState;
		renderer.material.color = GetStateColor();
	}


	Color GetStateColor()
	{
		switch(colorState) {
		case ColorState.INRANGE:
			return Color.green;
		case ColorState.DEFAULT:
		default:
			return defaultColor;
		}
	}
}
