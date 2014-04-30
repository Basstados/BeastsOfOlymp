using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BIniativeListUnit : MonoBehaviour, IComparable {

	public UILabel label;
	public UISprite unitPortrait;
	public UISprite teamSprite;
	public UISprite typeSprite;
	public Unit unit;

	float height;
	float firstYOffset;
	int positionIndex;
	Vector3 targetPostion;

	Color defaultColor = Color.white;
	Color activeColor = Color.green;
	Color deadColor = Color.black;
	Color aiColor = new Color(0.77255f, 0.21961f, 0.21961f);
	Color playerColor = new Color(0.25490f, 0.85882f, 0.23529f);

	public void Init(Unit unit, float height, float firstYOffset)
	{
		this.unit = unit;
		this.height = height;
		this.firstYOffset = firstYOffset;
		label.text = unit.UnitName;
		typeSprite.spriteName = unit.Element.elementName;
		unitPortrait.spriteName = unit.UnitName;

		if(unit.team == Unit.Team.AI) {
			teamSprite.color = aiColor;
		} else {
			teamSprite.color = playerColor;
		}
	}

//	public void SetActive() 
//	{
//		label.color = activeColor;
//	}

	public void SetDead()
	{
		teamSprite.color = deadColor;
	}

	public void Reset()
	{
		if(unit.Alive) {
			label.color = defaultColor;
		} else {
			SetDead();
		}

		if(unit.team == Unit.Team.AI) {
			teamSprite.color = aiColor;
		} else {
			teamSprite.color = playerColor;
		}
	}

	public void UpdatePosition(int newPositionIndex)
	{
		positionIndex = newPositionIndex;
		Vector3 posVec = transform.localPosition;
		posVec.y = - positionIndex * height;
		if(positionIndex == 0) {
			this.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
		} else {
			this.transform.localScale = Vector3.one;
			posVec.y -= firstYOffset;
		}

		targetPostion = posVec;
		StartCoroutine(MoveToPositionRoutine());
	}

	IEnumerator MoveToPositionRoutine()
	{
		Vector3 startPos = transform.localPosition;
		float t = 0;
		while(t < 1) {
			t += Time.deltaTime;
			transform.localPosition = Vector3.Lerp(startPos, targetPostion, t);
			// wait 1 frame
			yield return 0;
		}
		transform.localPosition = targetPostion;
	}

	/// <summary>
	/// Unit will be compared by there speed value
	/// </summary>
	/// <returns>Return value is less than 0 if this comes before obj when sorted. Value = 0 if both on the same sort position. Value > 0 if obj comes first.</returns>
	/// <param name="obj">Object.</param>
	public int CompareTo(object obj) 
	{
		if (obj == null)
			return 1;
		
		Unit otherUnit = (obj as BIniativeListUnit).unit;
		if (otherUnit != null) 
			return otherUnit.Initiative.CompareTo(this.unit.Initiative);
		else
			throw new ArgumentException("Object is not a Unit");
	}

	public void OnClick() 
	{
		Vector3 pos = new Vector3(this.unit.mapTile.x, 0, this.unit.mapTile.y);
		// BCameraMover is attached to the parent of the main camera
		// use BCameraMover.Focus(Vector3) to let the camera focus on the unit related to this InitListUnit
		Camera.main.transform.parent.GetComponent<BCameraMover>().Focus(pos);
	}
}
