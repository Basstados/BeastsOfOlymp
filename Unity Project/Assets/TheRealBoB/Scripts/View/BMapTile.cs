using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class BMapTile : MonoBehaviour {

	public Material defaultMaterial;
	public Material moveRangeMaterial;
	public Material attackRangeMaterial;
	public Material pathMaterial;
	public Material attackAreaMaterial;
	public Material clickableMaterial;

	public MapTile mapTile;
	public ToppingType topping; // the topping type wich will be instantiate on game start
	[HideInInspector]
	public ColorState colorState; // the current visual state of this instance

	public bool InRange {
		get {
			return !(colorState == ColorState.DEFAULT);
		}
	}

	public bool Clickable { get; set; }

	public enum ToppingType {
		NONE,
		SOLID_OBSTACLE
	}

	public enum ColorState {
		MOVERANGE,
		ATTACKRANGE,
		DEFAULT,
		PATH,
		CLICKABLE,
		ATTACKAREA
	}

	public void ChangeColorState(BMapTile.ColorState colorState)
	{
		StopCoroutine("TweenRoutine");
		this.colorState = colorState;

		switch(colorState) {
		case ColorState.MOVERANGE:
			renderer.sharedMaterial = moveRangeMaterial;
			this.Clickable = true;
			break;
		case ColorState.ATTACKRANGE:
			renderer.sharedMaterial = attackRangeMaterial;
			this.Clickable = true;
			break;
		case ColorState.PATH:
			renderer.sharedMaterial = pathMaterial;
			StartCoroutine(TweenRoutine(pathMaterial, Color.yellow, Color.gray));
			break;
		case ColorState.CLICKABLE:
			renderer.sharedMaterial = clickableMaterial;
			this.Clickable = true;
			break;
		case ColorState.ATTACKAREA:
			renderer.sharedMaterial = attackAreaMaterial;
			break;
		case ColorState.DEFAULT:
		default:
			renderer.sharedMaterial = defaultMaterial;
			this.Clickable = false;
			break;
		}
	}

	public void UpdateTopping()
	{
		switch(topping)
		{
		case ToppingType.NONE:
			mapTile.topping = null;
			break;
		case ToppingType.SOLID_OBSTACLE:
			mapTile.topping = new SolidObstacle();
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
