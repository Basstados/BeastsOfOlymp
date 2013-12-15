using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Database {

	static string basePath = "Assets/Resources/";
	static string attackCollectionPath = "AttackCollection";
	static string unitCollectionPath = "UnitCollection";
	static string mapDataPath = "MapData";

	AttackCollection atkCollection = new AttackCollection();
	UnitCollection unitCollection = new UnitCollection();
	MapData mapData = new MapData();

	#region singelton
	static Database instance;
	static Database Instance 
	{
		get {
			if(null == instance)
				instance = new Database();
			return instance;
		}
	}
	public Database() {
//		basePath = Application.dataPath + "/Resources/";
	}
	#endregion

	#region public
	public static void AddAttack(Attack attack)
	{
		Instance.atkCollection.Add(attack);
	}

	public static void AddAttackArray(Attack[] attacks)
	{
		foreach(Attack atk in attacks) {
			Instance.atkCollection.Add(atk);
		}
	}

	public static Attack GetAttack(string name)
	{
		return Instance.atkCollection.Get(name);
	}

	public static void AddUnitData(UnitData data)
	{
		Instance.unitCollection.Add(data);
	}

	public static void AddUnitDataArray(UnitData[] data)
	{
		foreach(UnitData item in data) {
			Instance.unitCollection.Add(item);
		}
	}

	public static UnitData GetUnitData(string name)
	{
		return Instance.unitCollection.Get(name);
	}

	public static void SetMapData(MapData data)
	{
		Instance.mapData = data;
	}

	public static MapData GetMapData()
	{
		return Instance.mapData;
	}

	public static void SaveAsFile()
	{
		string json;
		// save attack collection to file
		json = LitJson.JsonMapper.ToJson(Instance.atkCollection);
		File.WriteAllText(attackCollectionPath,json);
		// save unit collection to file
		json = LitJson.JsonMapper.ToJson(Instance.unitCollection);
		File.WriteAllText(unitCollectionPath,json);
		// save map data to file
		json = LitJson.JsonMapper.ToJson(Instance.mapData);
		File.WriteAllText(mapDataPath,json);
	}

	public static void LoadFromFile()
	{
		TextAsset attkCollJSON = (TextAsset) Resources.Load(attackCollectionPath, typeof(TextAsset));
		Instance.atkCollection = LitJson.JsonMapper.ToObject<AttackCollection>(attkCollJSON.text);

		TextAsset unitCollJSON = (TextAsset) Resources.Load(unitCollectionPath, typeof(TextAsset));
		Instance.unitCollection = LitJson.JsonMapper.ToObject<UnitCollection>(unitCollJSON.text);

		TextAsset mapDataJSON = (TextAsset) Resources.Load(mapDataPath, typeof(TextAsset));
		Instance.mapData = LitJson.JsonMapper.ToObject<MapData>(mapDataJSON.text);
	}
	#endregion
}
