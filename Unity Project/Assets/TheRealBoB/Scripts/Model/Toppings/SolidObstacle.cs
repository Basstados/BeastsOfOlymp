using System;
using UnityEngine;

public class SolidObstacle : Topping
{
	public SolidObstacle() : base()
	{
		isObstacle = true;
		isTargetable = false;
		isLinked = false;
		prefabName = "SOLID_OBSTACLE";
	}

	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// For the solid obstacle nothing will happen, since nobody can stand here.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit wich will effected by what ever will happen.</param>
	public override void OnStayEffect (Unit unit)
	{
		// do nothing
	}

	/// <summary>
	/// The effect hat will happen, if this field will be hit by an attack.
	/// Since the solid obstacle is indestructible nothing will happen, again.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack wich triggerd this effect.</param>
	public override void OnAttackEffect (Attack attack, Model model)
	{
		// do nothing either
	}
}