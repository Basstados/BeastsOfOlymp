using System;
using System.Collections.Generic;

[System.Serializable]
public class TypeCollection
{
	public Dictionary<string,Element> typeDict = new Dictionary<string, Element>();

	public void Add(Element type)
	{
		typeDict.Add(type.elementName, type);
	}

	public Element Get(string name)
	{
		return typeDict[name];
	}
}
