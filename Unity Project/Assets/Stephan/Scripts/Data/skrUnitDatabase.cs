using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class skrUnitDatabase
{
	List<skrUnitData> rows;
	Dictionary<string, skrUnitData> dict;

	public void GenerateDictionary() {
		dict = new Dictionary<string, skrUnitData> ();

		foreach (skrUnitData data in rows) {
			dict.Add(data.id, data);
		}
	}

	public skrUnitData GetData(string id)
	{
		return dict[id];
		// more clean:
		// c# indexer (overload operator)
	}
}
