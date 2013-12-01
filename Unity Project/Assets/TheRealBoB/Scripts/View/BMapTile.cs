using UnityEngine;
using System.Collections;

public class BMapTile : MonoBehaviour {

	public MapTile mapTile;
	public ColorState colorState;

	public enum ColorState {
		INRANGE,
		DEFAULT
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
			break;
		case ColorState.DEFAULT:
		default:
			return Color.white;
			break;
		}
	}
}
