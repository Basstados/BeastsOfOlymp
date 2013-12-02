using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackCollection 
{	
	public Dictionary<string,Attack> attackDict = new Dictionary<string,Attack>();

	
	public void AddAttack(Attack attack)
	{
		attackDict.Add(attack.name, attack);
	}

	public Attack GetAttack(string name)
	{
		return attackDict[name];
	}
}
