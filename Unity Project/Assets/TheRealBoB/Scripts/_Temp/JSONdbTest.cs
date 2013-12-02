using UnityEngine;
using System.Collections;
using System.IO;

public class JSONdbTest : MonoBehaviour {

	
	// Use this for initialization
	void Start () 
	{
		Attack atk1 = new Attack();
		atk1.name = "Tackle";
		atk1.hitChance = 0.9f;
		atk1.damage = 30;
		atk1.range = 1;
		atk1.apCost = 2;

		Attack atk2 = new Attack();
		atk2.name = "Stachelschuss";
		atk2.hitChance = 0.8f;
		atk2.damage = 20;
		atk2.range = 4;
		atk2.apCost = 2;

		Database.AddAttack(atk1);
		Database.AddAttack(atk2);

		string json = Database.ToJson();
		File.WriteAllText("Assets/Resources/AttackDatabase.json", json);
		Debug.Log("Json file written " + json.Length);


//		LitJson.JsonMapper.ToObject<Database>(File.ReadAllText("Assets/Resources/AttackDatabase.json"));

//		Debug.Log("Tackle dmg: " + Database.GetAttack("Tackle").damage);
	}
}
