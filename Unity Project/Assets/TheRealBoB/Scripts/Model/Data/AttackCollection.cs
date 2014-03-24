using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackCollection //: ScriptableObject
{	
	public Dictionary<string,Attack> attackDict = new Dictionary<string,Attack>();
	
	public void Add(Attack attack)
	{
		attackDict.Add(attack.attackName, attack);
	}

	public Attack Get(string name)
	{
		return attackDict[name];
	}
}
