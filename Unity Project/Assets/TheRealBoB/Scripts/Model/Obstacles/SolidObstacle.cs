using System;
using UnityEngine;

public class SolidObstacle : Topping
{
	/// <summary>
	/// The solid obstacle is always block, so it always returns true.
	/// </summary>
	/// <returns><c>true</c> if this instance is blocking; otherwise, <c>false</c>.</returns>
	public override bool IsBlocking ()
	{
		return true;
	}

	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// For the solid obstacle nothing will happen, since nobody can stand here.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit wich will effected by what ever will happen.</param>
	public override void OnStayEffect (MapTile mapTile, Unit unit)
	{
		// do nothing
	}

	/// <summary>
	/// The effect hat will happen, if this field will be hit by an attack.
	/// Since the solid obstacle is indestructible nothing will happen, again.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack wich triggerd this effect.</param>
	public override void OnAttackEffect (MapTile mapTile, Attack attack)
	{
		// do nothing either
	}
}