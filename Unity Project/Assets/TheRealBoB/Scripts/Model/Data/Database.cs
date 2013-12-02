using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Database {

	AttackCollection atkCollection;

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
		atkCollection = new AttackCollection();
	}
	#endregion

	#region external
	public static void AddAttack(Attack attack)
	{
		Instance._AddAttack(attack);
	}

	public static Attack GetAttack(string name){
		return Instance._GetAttack(name);
	}

	public static string ToJson()
	{
		return LitJson.JsonMapper.ToJson(Instance.atkCollection);
	}

	public static void LoadFromFile(string path)
	{
		Instance.atkCollection = LitJson.JsonMapper.ToObject<AttackCollection>(File.ReadAllText(path));
	}
	#endregion

	#region internal
	void _AddAttack(Attack attack)
	{
		atkCollection.AddAttack(attack);
	}

	Attack _GetAttack(string name)
	{
		return atkCollection.GetAttack(name);
	}
	#endregion
}
