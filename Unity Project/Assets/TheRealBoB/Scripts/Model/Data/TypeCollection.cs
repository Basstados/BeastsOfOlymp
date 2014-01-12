using System;
using System.Collections.Generic;

[System.Serializable]
public class TypeCollection
{
	public Dictionary<string,Type> typeDict = new Dictionary<string, Type>();

	public void Add(Type type)
	{
		typeDict.Add(type.name, type);
	}

	public Type Get(string name)
	{
		return typeDict[name];
	}
}
