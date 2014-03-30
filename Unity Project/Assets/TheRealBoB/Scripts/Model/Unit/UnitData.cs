using UnityEngine;
using System.Collections;

public class UnitData : ScriptableObject
{
	public string unitName = "newUnit";
	public Element element;
	public int baseHealth;
	public int baseAttack;
	public int baseInitiative;
	public int baseMoveRange;
	public Attack[] attacks;
}
