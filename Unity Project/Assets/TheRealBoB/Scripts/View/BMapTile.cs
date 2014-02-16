using UnityEngine;
using System.Collections;

public class BMapTile : MonoBehaviour {

	public Material defaultMaterial;
	public Material rangeMaterial;
	public Material pathMaterial;
	public Material attackAreaMaterial;
	public Material clickableMaterial;

	public MapTile mapTile;
	public ColorState colorState;

	public bool InRange {
		get {
			return !(colorState == ColorState.DEFAULT);
		}
	}

	public bool Clickable {
		get {
			return colorState == ColorState.CLICKABLE 
					|| colorState == ColorState.PATH 
					|| colorState == ColorState.ATTACKAREA;
		}
	}

	Color defaultColor;

	public enum ColorState {
		INRANGE,
		DEFAULT,
		PATH,
		CLICKABLE,
		ATTACKAREA
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
		case ColorState.CLICKABLE:
			renderer.sharedMaterial = clickableMaterial;
			break;
		case ColorState.ATTACKAREA:
			renderer.sharedMaterial = attackAreaMaterial;
			break;
		case ColorState.DEFAULT:
		default:
			renderer.sharedMaterial = defaultMaterial;
			break;
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
