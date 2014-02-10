using UnityEngine;
using System.Collections;

[System.Serializable]
public struct UnitData 
{
	public string name;
	public Type type;
	public int baseHealth;
	public int baseAttack;
	public int baseInitiative;
	public int baseMoveRange;
	public string[] attackNames;
}
