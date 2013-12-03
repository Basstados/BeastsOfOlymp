using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackCollection 
{	
	public Dictionary<string,Attack> attackDict = new Dictionary<string,Attack>();
	
	public void Add(Attack attack)
	{
		attackDict.Add(attack.name, attack);
	}

	public Attack Get(string name)
	{
		return attackDict[name];
	}
}
