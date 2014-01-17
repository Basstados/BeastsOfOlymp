using UnityEngine;
using System.Collections;

public class BMapTile : MonoBehaviour {

	public Material defaultMaterial;
	public Material rangeMaterial;
	public Material pathMaterial;

	public MapTile mapTile;
	public ColorState colorState;

	public bool InRange {
		get {
			return (colorState == ColorState.INRANGE || colorState == ColorState.PATH);
		}
	}

	Color defaultColor;

	public enum ColorState {
		INRANGE,
		DEFAULT,
		PATH
	}

	void Awake() 
	{
		defaultColor = renderer.material.color;
	}

	public void ChangeColorState(BMapTile.ColorState colorState)
	{
		StopCoroutine("TweenRoutine");
		this.colorState = colorState;

		switch(colorState) {
		case ColorState.INRANGE:
			renderer.sharedMaterial = rangeMaterial;
			break;
		case ColorState.PATH:
			renderer.sharedMaterial = pathMaterial;
			StartCoroutine(TweenRoutine(pathMaterial, Color.yellow, Color.gray));
			break;
		case ColorState.DEFAULT:
		default:
			renderer.sharedMaterial = defaultMaterial;
			break;
		}

//		if(colorState == ColorState.DEFAULT) {
//			renderer.sharedMaterial = defaultMaterial;
//		} else {
//			renderer.sharedMaterial = rangeMaterial;
//
//		}

		// renderer.sharedMaterial.color = GetStateColor();
	}


	Color GetStateColor()
	{
		switch(colorState) {
		case ColorState.INRANGE:
			return Color.green;
		case ColorState.PATH:
			return Color.yellow;
		case ColorState.DEFAULT:
		default:
			return defaultColor;
		}
	}

	IEnumerator TweenRoutine(Material mat, Color c1, Color c2)
	{
		float t = 0;
		bool rising = true;

		while(true) {
			if(rising) {
				t += Time.deltaTime;
				if(t>1) rising = false;
			} else {
				t -= Time.deltaTime;
				if(t<0) rising = true;
			}

			mat.color = Color.Lerp(c1,c2,t);
			// wait 1 frame
			yield return 0;
		}
	}
}
