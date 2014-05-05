using System.Collections;
using UnityEngine;

public abstract class Topping {

	/// <summary>
	/// Determines whether this topping in blocking the field or not.
	/// </summary>
	/// <returns><c>true</c> if this topping is blocking; otherwise, <c>false</c>.</returns>
	public abstract bool IsBlocking();
	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit wich will effected by what ever will happen.</param>
	public abstract void OnStayEffect(MapTile mapTile, Unit unit);
	/// <summary>
	/// The effect hat will happen, if this field will be hit by an attack.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack wich triggerd this effect.</param>
	public abstract void OnAttackEffect(MapTile mapTile, Attack attack);
}
