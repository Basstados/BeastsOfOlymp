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
	Color aiColor = Color.blue;
	Color playerColor = Color.cyan;

	public void Init(Unit unit)
	{
		this.unit = unit;
		label.text = unit.Name;
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
			sprite.color = aiColor;
		} else {
			sprite.color = playerColor;
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
