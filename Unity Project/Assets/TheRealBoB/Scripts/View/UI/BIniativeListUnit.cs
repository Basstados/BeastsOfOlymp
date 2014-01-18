using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BIniativeListUnit : MonoBehaviour, IComparable {

	public UILabel label;
	public UISprite teamSprite;
	public UISprite typeSprite;
	public Unit unit;

	float height;
	int positionIndex;
	Vector3 targetPostion;

	Color defaultColor = Color.white;
	Color activeColor = Color.green;
	Color deadColor = Color.black;
	Color aiColor = Color.grey;
	Color playerColor = Color.blue;

	public void Init(Unit unit, float height)
	{
		this.unit = unit;
		this.height = height;
		label.text = unit.Name;
		typeSprite.spriteName = unit.data.type.name;
	}

	public void SetActive() 
	{
		label.color = activeColor;
	}

	public void SetDead()
	{
		label.color = deadColor;
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

	/**
	 * Unit will be compared by there speed value
	 */ 
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
}
