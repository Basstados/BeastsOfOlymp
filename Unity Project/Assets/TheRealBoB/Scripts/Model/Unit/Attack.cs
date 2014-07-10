using UnityEngine;
using System.Collections;

public class Attack : ScriptableObject
{
	public string attackName = "NewAttack";
	public Element element;
	public float hitChance = 1;
	public int damage;
	public int range;

	public Vector[] area;

	public float effectDelay = 0.4f;
	public float hitDelay = 0.2f;
	public float fullAnimationTime = 1.2f;

	public bool IsRanged ()
	{
		if(range > 1)
		{
			return true;
		}

		return false;
	}
}
