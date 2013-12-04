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
		atks[0].hitChance = 0.9f;
		atks[0].damage = 30;
		atks[0].range = 1;
		atks[0].apCost = 2;

		atks[1] = new Attack();
		atks[1].name = "Stachelschuss";
		atks[1].hitChance = 0.8f;
		atks[1].damage = 20;
		atks[1].range = 4;
		atks[1].apCost = 2;

		Database.AddAttackArray(atks);
		#endregion

		#region units
		UnitData[] units = new UnitData[2];

		units[0].name = "Ratfratz";
		units[0].baseHealth = 100;
		units[0].baseAttack = 20;
		units[0].baseMovement = 5;
		units[0].baseInitiative = 10;
		units[0].baseActionPoints = 5;
		units[0].attackNames = new string[1]{"Tackle"};

		units[1].name = "Stachelschwein";
		units[1].baseHealth = 70;
		units[1].baseAttack = 15;
		units[1].baseMovement = 4;
		units[1].baseInitiative = 12;
		units[1].baseActionPoints = 5;
		units[1].attackNames = new string[1]{"Stachelschuss"};

		Database.AddUnitDataArray(units);
		#endregion

		#region map
		MapData map = new MapData();

		map.width = 10;
		map.height = 10;
		map.penaltys = new byte[map.width][];
		for (int i = 0; i < map.width; i++) {
			map.penaltys[i] = new byte[map.height];
			for (int j = 0; j < map.height; j++) {
				map.penaltys[i][j] = 1;
			}
		}
		map.teamUnits = new MapData.TeamUnit[2][];
		map.teamUnits[0] = new MapData.TeamUnit[2];
		map.teamUnits[0][0] = new MapData.TeamUnit("Stachelschwein",new Point(0,3));
		map.teamUnits[0][1] = new MapData.TeamUnit("Ratfratz",new Point(0,6));
		map.teamUnits[1] = new MapData.TeamUnit[2];
		map.teamUnits[1][0] = new MapData.TeamUnit("Stachelschwein",new Point(9,2));
		map.teamUnits[1][1] = new MapData.TeamUnit("Ratfratz",new Point(9,7));

		Database.SetMapData(map);
		#endregion

		Database.SaveAsFile();
	}
}
