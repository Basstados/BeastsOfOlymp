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
	public ArtificalIntelligence ai;


	bool canMove;
	bool canAttack;
	public string defaultAttack;
	int apUsed = 0;
	int currentHealth;

	#region properties
	public string	Name				{get{return this.data.name;}}
	public int 		Initiative 			{get{return this.data.baseInitiative;}}
	public int 		Movement 			{get{return this.data.baseMovement;}}
	public int		Attack				{get{return this.data.baseAttack;}}
	public int 		ActionPoints 		{get{return this.data.baseActionPoints - apUsed;}}
	public int		MaxActionPoints 	{get{return this.data.baseActionPoints;}}
	public int 		HealthPoints		{get{return this.currentHealth;}}
	public int		MaxHealthPoints		{get{return this.data.baseHealth;}}
	public bool 	AIControled			{get{return team == Team.AI;}}
	public bool 	Alive				{get{return (currentHealth > 0);}}
	public bool 	CanMove				{get{return (canMove && ActionPoints > 0);}
											set{canMove = value;}}
	public bool		CanAttack			{get{return (canAttack && ActionPoints > attacks[defaultAttack].apCost);}
											set{canAttack = value;}}
	#endregion
	
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
	}

	public void UseAP(int ap)
	{
		apUsed += ap;
	}

	public void LoseHealth (int damage)
	{
		currentHealth -= damage;
	}

	public void ResetTurn()
	{
		apUsed = 0;
		canMove = true;
		canAttack = true;
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
		return string.Format ("[Unit: Initiative={0}, Movement={1}, ActionPoints={2}, AIControled={3}]", Initiative, Movement, ActionPoints, AIControled);
	}
	
}


