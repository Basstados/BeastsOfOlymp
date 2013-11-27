using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Prototype_Combat : MonoBehaviour {

	private Prototype_Combat() {} // guarantee this will be always a singelton only - can't use the constructor!
	private static Prototype_Combat instance = null;

	public Prototype_Map map;
	public UILabel gameoverLabel;
	public List<Monster> monsterList;
	private int currentMonsterIndex = 0;

	private int player_monster = 0;
	private int ai_monster = 0;

	// Use this for initialization
	void Start () {
		instance = this;
		Debug.Log("Combat Start!");
		// fill up monster list
		GameObject[] monsterGOs = GameObject.FindGameObjectsWithTag("Monster");
		foreach( GameObject go in monsterGOs ) {
			Monster mon = go.GetComponent<Monster>();
			monsterList.Add( mon );
			if( mon.team == Monster.Team.PLAYER ) {
				player_monster++;
			} else {
				ai_monster++;
			}
		}

		// sort list by speed
		monsterList.Sort(
			delegate(Monster m1, Monster m2) {
				return m2.speed.CompareTo( m1.speed );
			}
		);

		// start combat
		StartCoroutine("NextRound");
	}

	private IEnumerator NextRound() {
		// wait 1 sec then start new round
		yield return new WaitForSeconds(1);
		currentMonsterIndex = -1;
		NextTurn();
	}

	public void NextTurn() {
		if( player_monster == 0 || ai_monster == 0 ) {
			return;
		}

		if( currentMonsterIndex < monsterList.Count-1 ) {
			// proceed with the next monster sorted by speed
			currentMonsterIndex++;
			Debug.Log("Next turn: " + monsterList[currentMonsterIndex].gameObject.name );
			monsterList[currentMonsterIndex].StartTurn();
		} else {
			// all monsters had its run, start new round
			StartCoroutine("NextRound");
		}
	}

	public void KillMonster(Monster mon) {
		monsterList.Remove( mon );
		map.QuadMatrix[(int) mon.CurrentPos.x, (int) mon.CurrentPos.y].penalty = 1;
		if( mon.team == Monster.Team.PLAYER ) {
			player_monster--;
		} else {
			ai_monster--;
		}
		Debug.Log(" Monster " + mon + " killed");
		Debug.Log(player_monster + " player monster and " + ai_monster + "ai monster left");

		if( player_monster == 0 || ai_monster == 0 ) {
			GameOver();
		}
	}

	void GameOver() {
		gameoverLabel.enabled = true;
		if( player_monster == 0 ) {
			gameoverLabel.text = "Niederlage!";
		} else {
			gameoverLabel.text = "Sieg!";
		}

		foreach( Monster mon in monsterList ) {
			mon.combatMenu.Hide();
		}
	}


	// Update is called once per frame
	void Update () {
	
	}

	public static Prototype_Combat Instance {
		get {
			if (instance == null) {
				instance = new Prototype_Combat();
			}
			return instance;
		}
	}
}
