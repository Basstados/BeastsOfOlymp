using System;

public class BurningOilField : Topping
{
    int damagePerTurn = 2;

    /// <summary>
    /// Constructor.
    /// Inistalize this instance.
    /// </summary>
    public BurningOilField() : base()
    {
        // How many round will this topping life?
        duration = 1;
        // Does units must move around this topping?
        isObstacle = true;
        // What is the name of the Prefab to be instantiate? (without ".prefab")
        prefabName = "BURNING_OIL"; // Prefab path is: Asset/Resources/Toppings/
    }

    /// <summary>
    /// The effect that will happen, if a unit stands on the field with this topping.
    /// Deal damage each turn to the unit on this burning oil.
    /// </summary>
    /// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
    /// <param name="unit">The unit which will affected by whatever will happen.</param>
    public override void OnStayEffect(Unit unit)
    {
        // damage the unit which stays on this mapTile
        unit.LoseHealth(damagePerTurn);
    }

    /// <summary>
    /// The effect that will happen, if this field will be hit by an attack.
    /// </summary>
    /// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
    /// <param name="attack">The attack which triggered this effect.</param>
    public override void OnAttackEffect(Attack attack)
    {
        // do something when hit by a water attack
        if (attack.element.elementName == "Wasser")
        {
            // remove this topping
            mapTile.topping = null;
        }
    }
}

