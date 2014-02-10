using UnityEngine;
using System.Collections;

public class DatabaseToFile : MonoBehaviour {

	
	// Use this for initialization
	void Start () 
	{
		#region attacks
		Attack[] atks = new Attack[2];

		atks[0] = new Attack();
		atks[0].name = "Tackle";
		atks[0].hitChance = 1;
		atks[0].damage = 0;
		atks[0].range = 1;
		atks[0].apCost = 2;

		atks[1] = new Attack();
		atks[1].name = "Stachelschuss";
		atks[1].hitChance = 1;
		atks[1].damage = 0;
		atks[1].range = 4;
		atks[1].apCost = 2;

		Database.AddAttackArray(atks);
		#endregion

		#region units
		UnitData[] units = new UnitData[2];

		units[0].name = "Minomon";
		units[0].baseHealth = 30;
		units[0].baseAttack = 3;
		units[0].baseInitiative = 8;
		units[0].attackNames = new string[1]{"Tackle"};

		units[1].name = "Speimon";
		units[1].baseHealth = 14;
		units[1].baseAttack = 6;
		units[1].baseInitiative = 7;
		units[1].attackNames = new string[1]{"Stachelschuss"};

		Database.AddUnitDataArray(units);
		#endregion

		#region map
		MapData map = new MapData();

		map.width = 10;
		map.height = 10;
		map.penalties = new byte[map.width][];
		for (int i = 0; i < map.width; i++) {
			map.penalties[i] = new byte[map.height];
			for (int j = 0; j < map.height; j++) {
				map.penalties[i][j] = 1;
			}
		}
		map.teamUnits = new MapData.TeamUnit[2][];
		map.teamUnits[0] = new MapData.TeamUnit[3];
		map.teamUnits[0][0] = new MapData.TeamUnit("Speimon",new Point(0,4));
		map.teamUnits[0][1] = new MapData.TeamUnit("Minomon",new Point(0,2));
		map.teamUnits[0][2] = new MapData.TeamUnit("Minomon",new Point(0,6));
		map.teamUnits[1] = new MapData.TeamUnit[3];
		map.teamUnits[1][0] = new MapData.TeamUnit("Speimon",new Point(9,5));
		map.teamUnits[1][1] = new MapData.TeamUnit("Minomon",new Point(9,3));
		map.teamUnits[1][2] = new MapData.TeamUnit("Minomon",new Point(9,7));

		Database.SetMapData(map);
		#endregion

		Debug.Log("Save GameData");
		Database.SaveAsFile();
	}
}
