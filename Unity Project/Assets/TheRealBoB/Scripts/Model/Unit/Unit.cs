using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Unit : IComparable
{
	public UnitData data = new UnitData();
	public MapTile mapTile { get; set; }
	public Team team;
	public Dictionary<string,Attack> attacks = new Dictionary<string, Attack>();
	public IArtificalIntelligence ai;

	public string defaultAttack;
	int apUsed = 0;
	int currentHealth;
	int maxAttackPoints = 1;
	int currentAttackPoints;
	int maxMovePoints = 1;
	int currentMovePoints;

	#region properties
	public string	Name				{get{return this.data.name;}}
	public int 		Initiative 			{get{return this.data.baseInitiative;}}
	public int		Attack				{get{return this.data.baseAttack;}}
	public int 		HealthPoints		{get{return this.currentHealth;}}
	public int		MaxHealthPoints		{get{return this.data.baseHealth;}}
	public bool 	AIControled			{get{return team == Team.AI;}}
	public bool 	Alive				{get{return (currentHealth > 0);}}
	public bool 	CanMove				{get{return (currentMovePoints > 0);}}
	public bool		CanAttack			{get{return (currentAttackPoints > 0);}}
	public int 		AttackPoints		{get{return this.currentAttackPoints;} set{currentAttackPoints = ClampInt(value,0,maxAttackPoints);}}
	public int 		MovePoints			{get{return this.currentMovePoints;} set{currentMovePoints = ClampInt(value,0,maxMovePoints);}}
	#endregion
	
	private int ClampInt(int value, int min, int max)
	{
		return (value < min) ? min : ((value > max) ? max : value);
	}

	public enum Team 
	{
		PLAYER = 0,
		AI = 1
	}

	public void Init(UnitData data)
	{
		this.data = data;
		foreach(string atkName in data.attackNames) {
			attacks.Add(atkName, Database.GetAttack(atkName));
		}

		defaultAttack = data.attackNames[0];
		currentHealth = data.baseHealth;
		maxMovePoints = data.baseMoveRange;
	}

	public void LoseHealth (int damage)
	{
		currentHealth -= damage;
	}

	public void ResetTurn()
	{
		apUsed = 0;
		currentMovePoints = maxMovePoints;
		currentAttackPoints = maxAttackPoints;
	}

	/**
	 * Unit will be compared by there speed value
	 */ 
	public int CompareTo(object obj) 
	{
		if (obj == null)
			return 1;

		Unit otherUnit = obj as Unit;
		if (otherUnit != null) 
			return otherUnit.Initiative.CompareTo(this.Initiative);
		else
			throw new ArgumentException("Object is not a Unit");
	}

	public override string ToString ()
	{
		return string.Format ("[Unit: data={0}, team={1}, attacks={2}, ai={3}, Name={4}, Initiative={5}, Attack={6}, HealthPoints={7}, MaxHealthPoints={8}, AIControled={9}, Alive={10}, CanMove={11}, CanAttack={12}, AttackPoints={13}, MovePoints={14}]", data, team, attacks, ai, Name, Initiative, Attack, HealthPoints, MaxHealthPoints, AIControled, Alive, CanMove, CanAttack, AttackPoints, MovePoints);
	}
}


