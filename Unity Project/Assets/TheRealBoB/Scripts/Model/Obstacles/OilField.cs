using System;

public class OilField : Topping
{

    public OilField(MapTile mapTile) : base(mapTile) {
        isObstacle = false;
        prefabName = "OIL";
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
	public override void OnAttackEffect(Attack attack)
	{
		// since elements are scriptable object, we need to check them via name - ugly :( 
		if(attack.element.elementName == "Feuer") {
			mapTile.topping = new BurningOilField(mapTile);
		}
	}
}


