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
        //duration = 3;
        // Does units must move around this topping?
        isObstacle = false;
		// Can this topping be attacked directly?
		isTargetable = true; // this should be true for all toppings with "OnAttackEffect"
		// Will the on attack effect spread out over neihgbour toppings?
		isLinked = false;
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

		// FIXME: this code should better be located inside the LoseHealth-method, 
		// but in order to avoid unknown consequences I kept using the code pattern found in CAttackUnit:96
		// when target died fire event AFTER burning was performed
		if(unit.HealthPoints <= 0) {
			// remove target from map
			unit.mapTile.unit = null;
			// fire event
			EventProxyManager.FireEvent(this, new UnitDiedEvent(unit));
		}
	}

    /// <summary>
    /// The effect that will happen, if this field will be hit by an attack.
    /// </summary>
    /// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
    /// <param name="attack">The attack which triggered this effect.</param>
	public override void OnAttackEffect(Attack attack, Model model)
    {
		// another example
		// if (mapTile.unit != null && attack.element.elementName == "Blitz")
		// 	 mapTile.unit.LoseHealth(10);

        // do something when hit by a water attack
        if (attack.element.elementName == "Wasser")
        {
            // remove this topping
            Destroy();
        }
    }
}

