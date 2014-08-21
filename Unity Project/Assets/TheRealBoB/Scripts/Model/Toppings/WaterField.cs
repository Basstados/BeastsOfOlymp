using System;

public class WaterField : Topping
{
	
	public WaterField() : base() {
		isObstacle = false;
		isTargetable = true;
		isLinked = true;
		prefabName = "Water";
	}
	
	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit wich will effected by what ever will happen.</param>
	public override void OnStayEffect(Unit unit)
	{
		// do nothing
	}
	
	/// <summary>
	/// The effect hat will happen, if this field will be hit by an attack.
	/// If attacked with fire, start burning.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack wich triggerd this effect.</param>
	public override void OnAttackEffect(Attack attack, Model model)
	{
		// since elements are scriptable object, we need to check them via name - ugly :( 
		//if(attack.element.elementName == "Grass") {
		if (mapTile.unit != null && attack.element.elementName == "lightning")
		{
			mapTile.unit.LoseHealth(4);

			//this.Destroy();
			/*mapTile.topping = new BurningOilField();
			mapTile.topping.Spawn(mapTile);*/
		}
	}
}