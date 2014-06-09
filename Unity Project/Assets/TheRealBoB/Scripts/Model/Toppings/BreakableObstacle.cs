using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BreakableObstacle : Topping
{
    public BreakableObstacle() : base() 
    {
        isObstacle = true;
		isTargetable = true;
		isLinked = false;

        prefabName = "BREAKABLE_OBSTACLE";
    }

    /// <summary>
    /// The effect that will happen, if a unit stands on the field with this topping.
    /// A unit can't stand on this field, so nothing happens here.
    /// </summary>
    /// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
    /// <param name="unit">The unit wich will effected by what ever will happen.</param>
    public override void OnStayEffect(Unit unit)
    {
        // do nothing
    }

    /// <summary>
    /// The effect hat will happen, if this field will be hit by an attack.
    /// Just remove this obstacle.
    /// </summary>
    /// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
    /// <param name="attack">The attack wich triggerd this effect.</param>
    public override void OnAttackEffect(Attack attack, Model model)
    {
        // remove this obstacle
        this.Destroy();
    }
}
