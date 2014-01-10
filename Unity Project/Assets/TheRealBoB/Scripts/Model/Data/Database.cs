using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public class Database {

	static string basePath = "Assets/Resources/";
	static string attackCollectionPath = "AttackCollection";
	static string unitCollectionPath = "UnitCollection";
	static string mapDataPath = "MapData";
	static string typeDataPath = "TypeData";

	AttackCollection atkCollection = new AttackCollection();
	UnitCollection unitCollection = new UnitCollection();
	TypeCollection typeCollection = new TypeCollection();
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
	#region attack
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

	public static void ClearAttacks()
	{
		Instance.atkCollection.attackDict.Clear();
	}

	public static Attack GetAttack(string name)
	{
		return Instance.atkCollection.Get(name);
	}

	public static Attack[] GetAttacks()
	{
		return Instance.atkCollection.attackDict.Values.ToArray();
	}
	#endregion

	#region unitData
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

	public static void ClearUnits ()
	{
		Instance.unitCollection.unitDict.Clear();
	}

	public static UnitData GetUnitData(string name)
	{
		return Instance.unitCollection.Get(name);
	}

	public static UnitData[] GetUnitsData()
	{
		return Instance.unitCollection.unitDict.Values.ToArray();
	}
	#endregion

	#region mapData
	public static void SetMapData(MapData data)
	{
		Instance.mapData = data;
	}

	public static MapData GetMapData()
	{
		return Instance.mapData;
	}
	#endregion

	#region type
	public static void AddType(Type type)
	{
		Instance.typeCollection.Add(type);
	}

	public static void AddTypeArray(Type[] types)
	{
		foreach (Type type in types) {
			Instance.typeCollection.Add(type);
		}
	}

	public static Type GetType(string name)
	{
		return Instance.typeCollection.Get(name);
	}

	public static Type[] GetTypes()
	{
		return Instance.typeCollection.typeDict.Values.ToArray();
	}

	public static void ClearTypes()
	{
		Instance.typeCollection.typeDict.Clear();
	}
	#endregion

	public static void SaveAsFile()
	{
		string json;
		// save attack collection to file
		json = LitJson.JsonMapper.ToJson(Instance.atkCollection);
		File.WriteAllText(basePath + attackCollectionPath + ".json",json);
		// save unit collection to file
		json = LitJson.JsonMapper.ToJson(Instance.unitCollection);
		File.WriteAllText(basePath + unitCollectionPath + ".json" ,json);
		// save map data to file
		json = LitJson.JsonMapper.ToJson(Instance.mapData);
		File.WriteAllText(basePath + mapDataPath + ".json" ,json);
		// save types to file
		json = LitJson.JsonMapper.ToJson(Instance.typeCollection);
		File.WriteAllText(basePath + typeDataPath + ".json" ,json);
	}

	public static void LoadFromFile()
	{
		// get json as TextAsset with Resources class
		// then convert TextAsset to collection object
		// unload asset to prefent caching in Unity Editor (we don't need it any longer anyway)

		TextAsset attkCollJSON = (TextAsset) Resources.Load(attackCollectionPath, typeof(TextAsset));
		if(attkCollJSON != null) {
			Instance.atkCollection = LitJson.JsonMapper.ToObject<AttackCollection>(attkCollJSON.text);
		} else {
			Instance.atkCollection = new AttackCollection();
		}
		Resources.UnloadAsset(attkCollJSON);

		TextAsset unitCollJSON = (TextAsset) Resources.Load(unitCollectionPath, typeof(TextAsset));
		if(unitCollJSON != null) {
			Instance.unitCollection = LitJson.JsonMapper.ToObject<UnitCollection>(unitCollJSON.text);
		} else {
			Instance.unitCollection = new UnitCollection();
		}
		Resources.UnloadAsset(unitCollJSON);

		TextAsset mapDataJSON = (TextAsset) Resources.Load(mapDataPath, typeof(TextAsset));
		if(mapDataJSON != null) {
			Instance.mapData = LitJson.JsonMapper.ToObject<MapData> (mapDataJSON.text);
		} else {
			Instance.mapData = new MapData();
		}
		Resources.UnloadAsset(mapDataJSON);

		TextAsset typeDataJSON = (TextAsset) Resources.Load(typeDataPath, typeof(TextAsset));
		if(typeDataJSON != null) {
			Instance.typeCollection = LitJson.JsonMapper.ToObject<TypeCollection> (typeDataJSON.text);
		} else {
			Instance.typeCollection = new TypeCollection();
		}
		Resources.UnloadAsset(typeDataJSON);
	}
	#endregion
}
