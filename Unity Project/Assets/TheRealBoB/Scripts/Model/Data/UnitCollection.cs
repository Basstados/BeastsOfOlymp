using System;
using System.Collections.Generic;

[System.Serializable]
public class UnitCollection
{
	public Dictionary<string,UnitData> unitDict = new Dictionary<string,UnitData>();

	public void Add(UnitData data)
	{
		unitDict.Add(data.name,data);
	}

	public UnitData Get(string name)
	{
		return unitDict[name];
	}
}

