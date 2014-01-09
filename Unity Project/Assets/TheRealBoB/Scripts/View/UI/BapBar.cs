using UnityEngine;
using System.Collections;

public class BapBar : MonoBehaviour {

	public GameObject apMarkerPrefab;
	public Color apFull;
	public Color apActive;
	public Color apEmpty;

	UISprite[] apSprites;
	int currentAP;

	public void Init(int maxAP)
	{
		currentAP = maxAP;
		apSprites = new UISprite[maxAP];
		float spriteWidth = apMarkerPrefab.GetComponent<UISprite>().localSize.x;
		float offsetX = - spriteWidth * maxAP / 2.0f;

		for (int i = 0; i < maxAP; i++) {
			// save reference
			GameObject handle = (GameObject) Instantiate(apMarkerPrefab);
			apSprites[i] = handle.GetComponent<UISprite>();
			// positioning
			handle.transform.parent = this.transform;
			handle.transform.localPosition = new Vector3(offsetX + i*spriteWidth,0);
			// fix wrong scale/rotation on instantiate UI elements [NGUI]
			handle.transform.localScale = Vector3.one;
			handle.transform.localRotation = Quaternion.identity;
			// set default color = full color
			apSprites[i].color = apFull;
		}
	}

	public void ActivateAP(int count)
	{
		for(int i = 0; i < count; i++) {
			apSprites[currentAP-i].color = apActive;	
		}
	}

	public void EmptyAP(int apLeft)
	{
		for(int i = 0; i < apSprites.Length; i++) {
			if(i < apLeft) {
				apSprites[i].color = apFull;
			} else {
				apSprites[i].color = apEmpty;
			}
		}
		currentAP = apLeft;
	}
}
