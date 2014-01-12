using UnityEngine;
using System.Collections;
using System.IO;

public class TesLitJson : MonoBehaviour {

	// Use this for initialization
	void Start () {
		skrUnit unit = ReadFromJson ();
		unit.speed += 100;
		Debug.Log (unit.speed);
		WriteToJson(unit);

		string json = LitJson.JsonMapper.ToJson (new MultiDimTest ());
		File.WriteAllText ("multidim.json", json);

		MultiDimTest mdt = LitJson.JsonMapper.ToObject<MultiDimTest> (File.ReadAllText ("multidim.json"));

		// path where you can you save all your staff
		// Application.persistentDataPath;
		//
		// for editor genereated JSON save it here
		// /Resources/ 

		foreach (int[] arr in mdt.arr) {
			foreach( int i in arr) 
				Debug.Log("i: " + i);
		}

		skrUnitData data = new skrUnitData();

		json = LitJson.JsonMapper.ToJson(data);
		File.WriteAllText ("unitdata.json", json);

		data = LitJson.JsonMapper.ToObject<skrUnitData> (File.ReadAllText ("unitdata.json"));
	}
	
	void WriteToJson(skrUnit unit) {
		unit.speed = 100;
		
		string json = LitJson.JsonMapper.ToJson(unit);
		File.WriteAllText ("test.json", json);
	}

	skrUnit ReadFromJson() {
		// class needs a default constructor!
		return LitJson.JsonMapper.ToObject<skrUnit>(File.ReadAllText("test.json"));
	}
}


public class MultiDimTest
{
	public int[][] arr;

	public MultiDimTest() {
		arr = new int[2][];

		for (int i = 0; i < arr.Length; i++) {
			arr[i] = new int[2];

			for (int j = 0; j < arr[i].Length; j++) {
				arr[i][j] = i + j;
			}
		}
	}
}