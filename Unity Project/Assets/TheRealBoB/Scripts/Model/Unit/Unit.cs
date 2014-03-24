using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Unit : IComparable
{
	public MapTile mapTile { get; set; }
	public Team team;
	public IArtificalIntelligence ai;

	// unit base attributes
	public int defaultAttackIndex;
	[SerializeField] UnitData data;

	// unit current attribute value of changing attributes
	int currentHealth;
	int maxAttackPoints = 1;
	int currentAttackPoints;
	int maxMovePoints = 1;
	int currentMovePoints;

	#region properties
	public string	UnitName			{get{return this.data.unitName;}}
	public int 		Initiative 			{get{return this.data.baseInitiative;}}
	public int		Attack				{get{return this.data.baseAttack;}}
	public int 		HealthPoints		{get{return this.currentHealth;}}
	public int		MaxHealthPoints		{get{return this.data.baseHealth;}}
	public Attack[] AttacksArray		{get{return this.data.attacks;}}
	public Element	Element				{get{return this.data.element;}}
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

	public void Init()
	{
		defaultAttackIndex = 0;
		currentHealth = data.baseHealth;
		maxMovePoints = data.baseMoveRange;
	}

	public void LoseHealth (int damage)
	{
		currentHealth -= damage;
	}

	public void ResetTurn()
	{
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
}


