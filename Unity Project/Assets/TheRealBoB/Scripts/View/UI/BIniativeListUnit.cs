using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BIniativeListUnit : MonoBehaviour, IComparable {

	public UILabel label;
	public UISprite sprite;

	public Unit unit;

	Color defaultColor = Color.white;
	Color activeColor = Color.green;
	Color deadColor = Color.grey;

	public void Init(Unit unit)
	{
		this.unit = unit;
	}

	public void SetActive() 
	{
		label.color = activeColor;
		sprite.color = activeColor;
	}

	public void SetDead()
	{
		label.color = deadColor;
		sprite.color = deadColor;
	}

	public void Reset()
	{
		if(unit.Alive) {
			label.color = defaultColor;
			sprite.color = defaultColor;
		} else {
			SetDead();
		}
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
